const express = require('express');
const app = express();
const db = require('./db');
const bodyParser = require('body-parser');

app.use(bodyParser.json());

app.use(express.json());

// app.get("/api", (req, res) => {
//   res.json({
//       "user": "Adrian is good"
//   });
// });

app.get("/api/authenticate/:id", (req, res) => {
  const id = req.params.id;
  const query = `SELECT * FROM attendees WHERE uid = ?`;

  db.query(query, [id], (error, results) => {
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

  db.query(query, (error, results) => {
    if (error) {
      console.log(error);
      res.status(500).json({ error: 'An error occurred while executing the query.' });
    } else {
      res.json({ data: results });
    }
  });
});



app.listen(5000, () => {
  console.log("QEAM Server running with from port 5000");
});