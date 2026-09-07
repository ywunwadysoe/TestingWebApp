import { useEffect, useRef, useState } from "react";
import {
  Alert,
  Avatar,
  Box,
  Button,
  Chip,
  IconButton,
  MenuItem,
  Select,
  Snackbar,
  Stack,
  Typography,
} from "@mui/material";
import {
  FiChevronDown,
  FiClock,
  FiMessageSquare,
  FiPlus,
  FiShield,
  FiSidebar,
} from "react-icons/fi";
import {
  getCustomerById,
  getCustomerByNrc,
  getCustomers,
} from "./api/customerApi";
import ChatInput from "./components/ChatInput";
import ChatMessage from "./components/ChatMessage";
import CustomerCard from "./components/CustomerCard";
import LoadingMessage from "./components/LoadingMessage";
import { useChat } from "./hooks/useChat";

const welcome = {
  role: "assistant",
  content:
    "Hello. I can help you understand the selected customer profile once an AI chat service is connected.",
  timestamp: new Date(),
};
function App() {
  const [authenticated, setAuthenticated] = useState(false);
  const [nrc, setNrc] = useState("");
  const [loggingIn, setLoggingIn] = useState(false);
  const [customers, setCustomers] = useState([]);
  const [customer, setCustomer] = useState(null);
  const [loadingCustomer, setLoadingCustomer] = useState(true);
  const [notice, setNotice] = useState("");
  const chatEnd = useRef(null);
  const customerId = customer?.customerId;
  const { messages, loading, sendMessage, clearMessages } = useChat(customerId, (error) =>
    setNotice(
      error.response?.data?.message ||
        "We could not reach the AI service. Please try again later.",
    ),
  );

  useEffect(() => {
    if (!authenticated) return;
    getCustomers()
      .then((list) => setCustomers(list))
      .catch(() =>
        setNotice(
          "Customer list is unavailable. You can still use the signed-in profile.",
        ),
      )
      .finally(() => setLoadingCustomer(false));
  }, [authenticated]);
  useEffect(() => {
    chatEnd.current?.scrollIntoView({ behavior: "smooth" });
  }, [messages, loading]);
  async function loadCustomer(id) {
    setLoadingCustomer(true);
    try {
      setCustomer(await getCustomerById(id));
    } catch {
      setNotice("We could not load that customer profile.");
    } finally {
      setLoadingCustomer(false);
    }
  }
  async function login(event) {
    event.preventDefault();
    if (!nrc.trim() || loggingIn) return;
    setLoggingIn(true);
    try {
      setCustomer(await getCustomerByNrc(nrc.trim()));
      setAuthenticated(true);
      setNotice("Customer profile loaded.");
    } catch (error) {
      setNotice(
        error.response?.status === 404
          ? "No customer was found for that NRC."
          : "We could not connect to the customer service. Check that the backend is running.",
      );
    } finally {
      setLoggingIn(false);
    }
  }
  const visibleMessages = messages.length ? messages : [welcome];

  if (!authenticated)
    return (
      <Box className="login-page">
        <Box className="login-panel">
          <Avatar className="login-mark">
            <FiShield />
          </Avatar>
          <Typography className="brand-name" sx={{ fontSize: 24, mb: 0.5 }}>
            MyNext AI
          </Typography>
          <Typography variant="body2" color="text.secondary" sx={{ mb: 4 }}>
            Customer assistant workspace
          </Typography>
          <Typography variant="h4" sx={{ fontFamily: "Space Grotesk", mb: 1 }}>
            Welcome back
          </Typography>
          <Typography color="text.secondary" sx={{ mb: 3 }}>
            Enter your NRC to securely load the matching customer profile.
          </Typography>
          <Box component="form" onSubmit={login}>
            <input
              className="nrc-input"
              value={nrc}
              onChange={(event) => setNrc(event.target.value)}
              placeholder="NRC number"
              autoComplete="off"
              autoFocus
              aria-label="NRC number"
            />
            <Button
              type="submit"
              fullWidth
              variant="contained"
              disabled={!nrc.trim() || loggingIn}
              className="new-chat login-button"
            >
              {loggingIn ? "Finding profile..." : "Continue with NRC"}
            </Button>
          </Box>
          <Typography
            variant="caption"
            color="text.secondary"
            sx={{ display: "block", mt: 3 }}
          >
            Uses the backend customer lookup by NRC. No password or fake
            authentication is added.
          </Typography>
        </Box>
      </Box>
    );

  return (
    <Box className="app-shell">
      <Box className="sidebar">
        <Stack
          direction="row"
          alignItems="center"
          spacing={1.25}
          className="brand"
        >
          <Avatar className="brand-mark">
            <FiShield />
          </Avatar>
          <div>
            <Typography className="brand-name">MyNext AI</Typography>
            <Typography className="brand-caption">
              Customer intelligence
            </Typography>
          </div>
        </Stack>
        <Button
          startIcon={<FiPlus />}
          fullWidth
          variant="contained"
          className="new-chat"
          onClick={clearMessages}
        >
          New chat
        </Button>
        <Typography className="side-label">CUSTOMER</Typography>
        {/* <Select
          fullWidth
          size="small"
          value={customer?.customerId || ""}
          onChange={(event) => loadCustomer(event.target.value)}
          IconComponent={FiChevronDown}
          disabled={!customers.length || loadingCustomer}
        >
          {customers.map((item) => (
            <MenuItem key={item.customerId} value={item.customerId}>
              {item.customerId}{" "}
              <Typography
                component="span"
                color="text.secondary"
                sx={{ ml: 1 }}
              >
                ({item.nrc})
              </Typography>
            </MenuItem>
          ))}
        </Select> */}
        <CustomerCard customer={customer} />
        <Box className="sidebar-bottom">
          <Stack direction="row" spacing={1} alignItems="center">
          </Stack>
        </Box>
      </Box>
      <Box className="main-panel">
        <Box className="topbar">
          <IconButton className="mobile-menu">
            <FiSidebar />
          </IconButton>
          <div>
            <Typography variant="h5">MyNext AI</Typography>
            <Typography variant="body2" color="text.secondary">
              Your personal customer assistant
            </Typography>
          </div>
        </Box>
        <Box className="chat-area">
          <Box className="chat-intro">
            <Typography variant="h4">Good morning.</Typography>
            <Typography color="text.secondary">
              A focused view of customer context, ready for your next
              conversation.
            </Typography>
          </Box>
          <Box className="messages">
            {loadingCustomer ? (
              <LoadingMessage />
            ) : (
              visibleMessages.map((message, index) => (
                <ChatMessage
                  message={message}
                  key={`${message.timestamp.toISOString()}-${index}`}
                />
              ))
            )}
            {loading && <LoadingMessage />}
            <div ref={chatEnd} />
          </Box>
        </Box>
        <Box className="composer">
          <ChatInput
            onSend={sendMessage}
            loading={loading}
            disabled={!customerId}
          />
        </Box>
      </Box>
      <Snackbar
        open={Boolean(notice)}
        autoHideDuration={6000}
        onClose={() => setNotice("")}
      >
        <Alert severity="info" onClose={() => setNotice("")} variant="filled">
          {notice}
        </Alert>
      </Snackbar>
    </Box>
  );
}
export default App;
