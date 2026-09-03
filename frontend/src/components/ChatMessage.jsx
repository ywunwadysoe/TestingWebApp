import { Avatar, Box, Paper, Stack, Typography } from "@mui/material";
import { FiCpu, FiUser } from "react-icons/fi";

function ChatMessage({ message }) {
  const isUser = message.role === "user";
  return (
    <Stack
      direction="row"
      spacing={1.5}
      justifyContent={isUser ? "flex-end" : "flex-start"}
      className="message-enter"
    >
      {!isUser && (
        <Avatar className="assistant-avatar">
          <FiCpu />
        </Avatar>
      )}
      <Box className={isUser ? "message-wrap user-message" : "message-wrap"}>
        <Paper className="message-bubble" elevation={0}>
          <Typography
            variant="body2"
            sx={{ whiteSpace: "pre-wrap", lineHeight: 1.65 }}
          >
            {message.content}
          </Typography>
        </Paper>
        <Typography className="message-time" variant="caption">
          {message.timestamp.toLocaleTimeString([], {
            hour: "2-digit",
            minute: "2-digit",
          })}
        </Typography>
      </Box>
      {isUser && (
        <Avatar className="user-avatar">
          <FiUser />
        </Avatar>
      )}
    </Stack>
  );
}

export default ChatMessage;
