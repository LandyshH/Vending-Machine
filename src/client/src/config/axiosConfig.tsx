import axios from 'axios';

export default axios.create({
    baseURL: 'http://localhost:7158/api', 
    timeout: 10000,
});

