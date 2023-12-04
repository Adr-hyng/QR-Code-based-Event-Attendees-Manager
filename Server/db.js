require('dotenv').config();
const mysql = require('mysql2');

const SCHEMA_NAME = process.env.DB_NAME

const connection = mysql.createConnection({
  host: process.env.DB_HOST,
  user: process.env.DB_USER,
  password: process.env.DB_PASSWORD,
  database: SCHEMA_NAME,
  ssl: false
});

connection.connect((err) => {
  if (err) throw err;
  console.log('Connected to MySQL!');
});

module.exports = {
  connection: connection,
  SCHEMA_NAME: SCHEMA_NAME
};
