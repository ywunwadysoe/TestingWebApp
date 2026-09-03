import {
  Box,
  Card,
  CardContent,
  Chip,
  Divider,
  Grid,
  Stack,
  Typography,
} from "@mui/material";
import {
  FiActivity,
  FiBriefcase,
  FiCreditCard,
  FiMapPin,
  FiUser,
} from "react-icons/fi";

const money = (value, currency = "") =>
  `${currency} ${Number(value || 0).toLocaleString()}`.trim();
function Field({ icon, label, value }) {
  return (
    <Stack direction="row" spacing={1.25} alignItems="flex-start">
      <span className="field-icon">{icon}</span>
      <div>
        <Typography variant="caption" color="text.secondary">
          {label}
        </Typography>
        <Typography variant="body2" fontWeight={600}>
          {value || "Not provided"}
        </Typography>
      </div>
    </Stack>
  );
}

export default function CustomerCard({ customer }) {
  if (!customer) return null;
  const behavior = customer.customerBehaviour;
  return (
    <Card className="customer-card">
      <CardContent>
        <Stack
          direction="row"
          justifyContent="space-between"
          alignItems="flex-start"
          mb={2}
        >
          <div>
            <Typography variant="overline" color="primary" fontWeight={700}>
              Customer profile
            </Typography>
            <Typography variant="h6">
              {customer.name || "Unnamed customer"}
            </Typography>
          </div>
          <Chip label={customer.customerId} size="small" className="id-chip" />
        </Stack>
        <Grid container spacing={2}>
          <Grid size={{ xs: 6 }}>
            <Field
              icon={<FiUser />}
              label="Age"
              value={customer.age ? `${customer.age} years` : ""}
            />
          </Grid>
          <Grid size={{ xs: 6 }}>
            <Field
              icon={<FiMapPin />}
              label="Location"
              value={customer.location}
            />
          </Grid>
          <Grid size={{ xs: 6 }}>
            <Field
              icon={<FiBriefcase />}
              label="Occupation"
              value={customer.occupation}
            />
          </Grid>
          <Grid size={{ xs: 6 }}>
            <Field
              icon={<FiCreditCard />}
              label="Monthly income"
              value={
                customer.monthlyIncome ? money(customer.monthlyIncome) : ""
              }
            />
          </Grid>
        </Grid>
        <Divider sx={{ my: 2 }} />
        <Stack direction="row" spacing={1} flexWrap="wrap" useFlexGap>
          <Chip
            size="small"
            label={`${customer.policies?.length || 0} policies`}
          />
          <Chip
            size="small"
            label={`${customer.paymentHistory?.length || 0} payments`}
          />
          <Chip
            size="small"
            label={
              customer.preferredContactChannel ||
              "Contact preference unavailable"
            }
          />
        </Stack>
        {behavior && (
          <Box mt={2} className="activity-strip">
            <Stack direction="row" spacing={1} alignItems="center">
              <FiActivity />
              <Typography variant="caption" fontWeight={700}>
                Recent behavior
              </Typography>
            </Stack>
            <Typography variant="body2">
              {behavior.loginCountLast30Days || 0} logins in the last 30 days
              {behavior.lastViewedPolicy
                ? ` · Last viewed ${behavior.lastViewedPolicy}`
                : ""}
            </Typography>
          </Box>
        )}
      </CardContent>
    </Card>
  );
}
