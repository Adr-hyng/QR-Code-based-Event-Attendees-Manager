const axios = require('axios');

async function updateAttendeeCheckIn(id, column, value) {
  try {
    const response = await axios.post(`http://localhost:5000/api/update_attendee/${id}`, {
      column: column,
      value: value
    });

    if (response.status === 200) {
      console.log('Attendee updated successfully.');
      console.log(response.data);
    } else {
      console.log('Failed to update attendee.');
      console.log(response.data);
    }
  } catch (error) {
    console.error('An error occurred while making the request.');
    console.error(error.message);
  }
}

// Example usage
const id = '5kdIBZbdnNdF7amvWX1sPk';
const column = 'amd2';
const value = new Date().toISOString();

updateAttendeeCheckIn(id, column, value);
