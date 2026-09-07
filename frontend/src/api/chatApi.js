import axiosClient from './axiosClient'

export async function sendChatMessage(message, customerId) {
  const { data } = await axiosClient.post('/api/chat', { message, customerId })
  return data
}
