import axiosClient from './axiosClient'

export async function sendChatMessage(message, customerId) {
  const endpoint = import.meta.env.VITE_AI_CHAT_ENDPOINT

  if (!endpoint) {
    const error = new Error('The backend does not currently expose an AI chat endpoint.')
    error.code = 'CHAT_ENDPOINT_NOT_CONFIGURED'
    throw error
  }

  const { data } = await axiosClient.post(endpoint, { message, customerId })
  return data
}
