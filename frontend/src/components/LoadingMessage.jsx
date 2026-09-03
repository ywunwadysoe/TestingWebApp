import { Avatar, Box, CircularProgress, Stack, Typography } from '@mui/material'
import { FiCpu } from 'react-icons/fi'

export default function LoadingMessage() {
  return <Stack direction="row" spacing={1.5} alignItems="flex-start" className="message-enter"><Avatar className="assistant-avatar"><FiCpu /></Avatar><Box className="loading-bubble"><CircularProgress size={15} /><Typography variant="caption">Reviewing customer context...</Typography></Box></Stack>
}
