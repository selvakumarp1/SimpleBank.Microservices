const BASE_URL = "/api/gateway";//For https
//const BASE_URL = "http://a1c9a2c0563cf40098cf94958de56fb8-1348290131.ap-southeast-1.elb.amazonaws.com/api/gateway";//works non https
//const BASE_URL = "http://localhost:5000/api/gateway";//for local before Kubernetes, Docker
// ✅ GET
export async function getCustomers() {
  const res = await fetch(`${BASE_URL}/customers`);
  return res.json();
}

export async function getAccounts() {
  const res = await fetch(`${BASE_URL}/accounts`);
  return res.json();
}

export async function getTransactions() {
  const res = await fetch(`${BASE_URL}/transactions`);
  return res.json();
}

// ✅ POST
export async function createTransaction(data) {
  const res = await fetch(`${BASE_URL}/transactions`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json"
    },
    body: JSON.stringify(data)
  });

  return res.json();
}