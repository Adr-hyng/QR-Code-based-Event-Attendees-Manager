const express = require('express');
const app = express();
const db = require('./db');
const bodyParser = require('body-parser');
const os = require('os');

const PORT = 5000;

app.use(bodyParser.json());

app.use(express.json());

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
  const networkInterfaces = os.networkInterfaces();
  let localHostAddress;
  if ("Wi-Fi" in networkInterfaces) {
    localHostAddress = networkInterfaces['Wi-Fi'][3]['address'];
  } else {
    localHostAddress = networkInterfaces["Loopback Pseudo-Interface 1"][1]["address"]
  }
  res.json({ address: localHostAddress, port: PORT });
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
  let localHostAddress;
  if("Wi-Fi" in networkInterfaces) {
    localHostAddress = networkInterfaces['Wi-Fi'][3]['address'];
  } else {
    localHostAddress = networkInterfaces["Loopback Pseudo-Interface 1"][1]["address"]
  }
  console.log(`QEAM Server from ${localHostAddress}:${PORT} running loaded with Database Schema: ${db.SCHEMA_NAME}`);
});