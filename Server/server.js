const express = require('express');
const app = express();
const db = require('./db');
const bodyParser = require('body-parser');
const os = require('os');
const { Console } = require('console');

const PORT = 5000;

app.use(bodyParser.json());

app.use(express.json());


function getIPAddress() {
  const { networkInterfaces } = require('os');

  const nets = networkInterfaces();
  const results = Object.create(null); // Or just '{}', an empty object

  for (const name of Object.keys(nets)) {
    for (const net of nets[name]) {
        // Skip over non-IPv4 and internal (i.e. 127.0.0.1) addresses
        // 'IPv4' is in Node <= 17, from 18 it's a number 4 or 6
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


app.get("/api/authenticate/:id", (req, res) => {
  const id = req.params.id;
  const query = `SELECT * FROM attendees WHERE uid = ?`;

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

app.post("/api/update_attendee/:id", (req, res) => {
  const id = req.params.id;
  const column = req.body.column;
  const value = req.body.value;

  const query = `UPDATE attendees SET ${column} = ? WHERE uid = ?`;

  db.connection.query(query, [value, id], (error, results) => {
    if (error) {
      console.log(error);
      res.status(500).json({ error: 'An error occurred while executing the query.' });
    } else if (results.affectedRows > 0) {
      res.json({ success: true });
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
      console.log("Desired key:", key);
      localHostIP = value;
    }
  }
  console.log(`QEAM Server from ${localHostIP}:${PORT} running loaded with Database Schema: ${db.SCHEMA_NAME}`);
});