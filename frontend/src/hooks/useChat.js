import { useRef, useState } from 'react'
import { sendChatMessage } from '../api/chatApi'

export function useChat(customerId, onError) {
  const [messages, setMessages] = useState([])
  const [loading, setLoading] = useState(false)
  const scrollTarget = useRef(null)

  async function sendMessage(text) {
    const message = text.trim()
    if (!message || loading) return false

    setMessages((current) => [...current, { role: 'user', content: message, timestamp: new Date() }])
    setLoading(true)
    try {
      const response = await sendChatMessage(message, customerId)
      const content = response?.message || response?.content || response?.answer
      if (!content) throw new Error('The AI service returned an empty response.')
      setMessages((current) => [...current, { role: 'assistant', content, timestamp: new Date() }])
      return true
    } catch (error) {
      onError(error)
      return false
    } finally {
      setLoading(false)
    }
  }

  function clearMessages() {
    setMessages([])
  }

  return { messages, loading, sendMessage, clearMessages, scrollTarget }
}
