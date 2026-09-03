import React from 'react'
import ReactDOM from 'react-dom/client'
import { CssBaseline, ThemeProvider, createTheme } from '@mui/material'
import App from './App'
import './styles.css'

const theme = createTheme({
  palette: {
    primary: { main: '#1261a0', dark: '#0b3d63' },
    secondary: { main: '#e68a3f' },
    background: { default: '#edf4f6', paper: '#ffffff' },
    text: { primary: '#17324d', secondary: '#658095' },
  },
  typography: {
    fontFamily: '"DM Sans", "Segoe UI", sans-serif',
    h1: { fontFamily: '"Space Grotesk", "DM Sans", sans-serif', fontWeight: 700 },
    h2: { fontFamily: '"Space Grotesk", "DM Sans", sans-serif', fontWeight: 700 },
    h3: { fontFamily: '"Space Grotesk", "DM Sans", sans-serif', fontWeight: 700 },
  },
  shape: { borderRadius: 16 },
  components: {
    MuiButton: { defaultProps: { disableElevation: true } },
    MuiCard: { styleOverrides: { root: { boxShadow: '0 12px 32px rgba(19, 59, 84, 0.08)' } } },
  },
})

ReactDOM.createRoot(document.getElementById('root')).render(
  <React.StrictMode>
    <ThemeProvider theme={theme}>
      <CssBaseline />
      <App />
    </ThemeProvider>
  </React.StrictMode>,
)
