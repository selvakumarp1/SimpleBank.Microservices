const BASE_URL = "/api/gateway";

// ✅ Helper to attach token
function getAuthHeaders() {
  const token = localStorage.getItem("token");

  return {
    "Content-Type": "application/json",
    "Authorization": `Bearer ${token}`
  };
}

// ✅ GET CUSTOMERS
export async function getCustomers() {
  const res = await fetch(`${BASE_URL}/customers`, {
    method: "GET",
    headers: getAuthHeaders()
  });

  if (!res.ok) throw new Error("Failed to fetch customers");

  return res.json();
}

// ✅ GET ACCOUNTS
export async function getAccounts() {
  const res = await fetch(`${BASE_URL}/accounts`, {
    method: "GET",
    headers: getAuthHeaders()
  });

  if (!res.ok) throw new Error("Failed to fetch accounts");

  return res.json();
}

// ✅ GET TRANSACTIONS
export async function getTransactions() {
  const res = await fetch(`${BASE_URL}/transactions`, {
    method: "GET",
    headers: getAuthHeaders()
  });

  if (!res.ok) throw new Error("Failed to fetch transactions");

  return res.json();
}

// ✅ CREATE TRANSACTION
export async function createTransaction(data) {
  const res = await fetch(`${BASE_URL}/transactions`, {
    method: "POST",
    headers: getAuthHeaders(),
    body: JSON.stringify(data)
  });

  if (!res.ok) throw new Error("Transaction failed");

  return res.json();
}