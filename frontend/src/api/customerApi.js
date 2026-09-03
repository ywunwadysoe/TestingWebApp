import axiosClient from './axiosClient'

export async function getCustomers() {
  const { data } = await axiosClient.get('/api/customers')
  return data
}

export async function getCustomerById(customerId) {
  const { data } = await axiosClient.get(`/api/customers/${encodeURIComponent(customerId)}`)
  return data
}

export async function getCustomerByNrc(nrc) {
  const { data } = await axiosClient.get(`/api/customers/details/by-nrc/${encodeURIComponent(nrc)}`)
  return data
}
