import axios from 'axios';

async function fetchData(IpAddress, id) {
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
      console.log("Full Name:", fullName);
    } else {
      console.log("Request was not successful or data is missing.");
    }
  } catch (error) {
    console.error("Error during the fetch operation:", error);
  }
}

// Call the async function
fetchData();
