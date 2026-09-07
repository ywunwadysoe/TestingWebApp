import { IconButton, InputAdornment, TextField, Tooltip } from "@mui/material";
import { FiArrowUp } from "react-icons/fi";
import { useState } from "react";

function ChatInput({ onSend, loading, disabled }) {
  const [value, setValue] = useState("");
  function submit() {
    if (!value.trim() || loading) return;
    onSend(value).then((sent) => {
      if (sent) setValue("");
    });
  }
  function onKeyDown(event) {
    if (event.key === "Enter" && !event.shiftKey) {
      event.preventDefault();
      submit();
    }
  }

  return (
    <TextField
      fullWidth
      multiline
      maxRows={4}
      value={value}
      onChange={(event) => setValue(event.target.value)}
      onKeyDown={onKeyDown}
      placeholder={
        disabled
          ? "Select a customer to start chatting"
          : "Ask about this customer..."
      }
      disabled={loading}
      InputProps={{
        endAdornment: (
          <InputAdornment position="end">
            <Tooltip title="Send message">
              <span>
                <IconButton
                  onClick={submit}
                  disabled={!value.trim() || loading}
                  color="primary"
                  className="send-button"
                >
                  <FiArrowUp />
                </IconButton>
              </span>
            </Tooltip>
          </InputAdornment>
        ),
      }}
      aria-label="Ask your customer assistant"
    />
  );
}

export default ChatInput;
