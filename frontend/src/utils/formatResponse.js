export function formatCustomerContext(customer) {
  if (!customer) return ''
  const policyNames = (customer.policies || []).map((policy) => policy.productName).filter(Boolean)
  return `Customer ${customer.name || customer.customerId} has ${customer.policies?.length || 0} active record(s).${policyNames.length ? ` Products: ${policyNames.join(', ')}.` : ''}`
}
