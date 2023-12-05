const express = require('express');
const app = express();
const axios = require("axios");
const db = require('./db');
const bodyParser = require('body-parser');
const os = require('os');
const fs = require('fs');

const PORT = 5000;

app.use(bodyParser.json());

app.use(express.json());

function getIPAddress() {
  const { networkInterfaces } = require('os');

  const nets = networkInterfaces();
  const results = Object.create(null); // Or just '{}', an empty object

  for (const name of Object.keys(nets)) {
    for (const net of nets[name]) {
        const familyV4Value = typeof net.family === 'string' ? 'IPv4' : 4
        if (net.family === familyV4Value && !net.internal) {
            if (!results[name]) {
                results[name] = [];
            }
            results[name].push(net.address);
        }
    }
  }
  return results;
}

async function fetchInformation(IpAddress, id) {
  const endpoint = `http://${IpAddress}:5000/api/authenticate/${id}`;

  try {
    // Make a GET request to the endpoint
    const response = await axios.get(endpoint);

    // Check if the response is successful (status code 200)
    if (response.status !== 200) {
      throw new Error(`HTTP error! Status: ${response.status}`);
    }

    // Access the data from the response
    const data = response.data;

    // Check if the request was successful
    if (data.success && data.data && data.data.length > 0) {
      // Extract the required fields from the data
      const user = data.data[0];
      const { fn, mi, ln } = user;

      // Concatenate the names with appropriate spacing
      const fullName = mi ? `${fn} ${mi} ${ln}` : `${fn} ${ln}`;

      // Output the concatenated full name
      return { name: fullName, ...user};
    } else {
      console.log("Request was not successful or data is missing.");
      return ""; // or return some default value if needed
    }
  } catch (error) {
    console.error("Error during the fetch operation:", error);
    return ""; // or return some default value if needed
  }
}


function appendToFile(filePath, content) {
  try {
    let existingContent = '';

    // Check if the file exists
    if (fs.existsSync(filePath)) {
      // If the file exists, read its content
      existingContent = fs.readFileSync(filePath, 'utf-8');
    }

    // Check if the content already exists
    if (!existingContent.includes(content)) {
      // If the content is not present, append it with a newline
      fs.appendFileSync(filePath, `${content}\n`);
    } 
  } catch (error) {
    console.error(`Error appending to or creating file ${filePath}:`, error);
  }
}

app.get("/api/authenticate/:id", (req, res) => {
  const id = req.params.id;
  const query = `SELECT * FROM attendees WHERE BINARY uid = ?`;
 
  db.connection.query(query, [id], (error, results) => {
   if (error) {
     console.log(error);
     res.status(500).json({ error: 'An error occurred while executing the query.' });
   } else if (results.length > 0) {
     res.json({ success: true, data: results });
   } else {
     res.status(401).json({ success: false });
   }
  });
 });
 

app.get("/api/attendees", (req, res) => {
  const query = `SELECT * FROM attendees`;

  db.connection.query(query, (error, results) => {
    if (error) {
      console.log(error);
      res.status(500).json({ error: 'An error occurred while executing the query.' });
    } else {
      res.json({ data: results });
    }
  });
});

app.get("/api/status_info", (req, res) => {
  res.json(getIPAddress());
});

let oldTimeLog = new Map();
app.post("/api/update_attendee/:id", (req, res) => {
  const id = req.params.id;
  const column = req.body.column;
  const value = req.body.value;

  const query = `UPDATE attendees SET ${column} = COALESCE(${column}, ?) WHERE uid = ?`;

  db.connection.query(query, [value, id], async (error, results) => {
    if (error) {
      console.log(error);
      res.status(500).json({ error: 'An error occurred while executing the query.' });
    } else if (results.affectedRows > 0) {
      res.json({ success: true });

      const regex = /(checkin|checkout)/;
      const newColumn = column.match(regex);
      const profile = await fetchInformation("127.0.0.1", id);

      const oldLog = oldTimeLog.get(profile.id);
      oldTimeLog.set(profile.id, Date.now());
      if((oldLog + 5000) >= Date.now()) return;
      let currentActivity = newColumn ? newColumn[0] : "undefined"

      const activity = currentActivity.includes("checkin") ? currentActivity.replace("checkin", "Checked-IN") : (currentActivity.includes("checkout")) ? currentActivity.replace("checkout", "Checked-Out") : "undefined";

      appendToFile("logs.txt", `[${profile[column]}] ${profile.name} has ${activity}`);
    } else {
      res.status(404).json({ success: false, error: 'No attendee found with the given id.' });
    }
  });
});

app.listen(PORT, () => {
  const networkInterfaces = os.networkInterfaces();
  let localHostAddress = getIPAddress();
  let localHostIP;
  const connectionRegex = /Local Area Connection.*|Wi-Fi/;
  for (const [key, value] of Object.entries(localHostAddress)) {
    if (connectionRegex.test(key)) {
      localHostIP = value;
    }
  }
  console.log(`QEAM Server from ${localHostIP}:${PORT} running loaded with Database Schema: ${db.SCHEMA_NAME}`);
});