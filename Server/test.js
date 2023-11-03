const axios = require('axios');

async function testUpdateAttendee2() {
  const id = 'NzmW19gA3ER4GNxwx8JOKh';
  const column = 'pmd1';
  const value = new Date(); // Replace this with your desired DateTime value

  try {
    const response = await axios.post(`http://localhost:5000/api/update_attendee/${id}`, {
      column: column,
      value: value.toISOString() // Convert the DateTime value to ISO 8601 format
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
    console.error(error.response.data);
  }
}

testUpdateAttendee2();


