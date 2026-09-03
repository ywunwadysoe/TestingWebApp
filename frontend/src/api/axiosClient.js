import axios from 'axios'

const axiosClient = axios.create({
  baseURL: import.meta.env.VITE_API_BASE_URL || 'https://localhost:44368/',
  timeout: 10000,
  headers: { 'Content-Type': 'application/json' },
})

export default axiosClient
