// API base URL
const API_BASE_URL = "http://localhost:5272/api";

// Utility functions
function showMessage(message, type = "success") {
  const messageDiv = document.getElementById("message");
  if (messageDiv) {
    messageDiv.textContent = message;
    messageDiv.className = `message ${type}`;
    setTimeout(() => {
      messageDiv.textContent = "";
      messageDiv.className = "message";
    }, 5000);
  }
}

function getAuthToken() {
  return localStorage.getItem("authToken");
}

function setAuthToken(token) {
  localStorage.setItem("authToken", token);
}

function removeAuthToken() {
  localStorage.removeItem("authToken");
}

function getAuthHeaders() {
  const token = getAuthToken();
  return token ? { Authorization: `Bearer ${token}` } : {};
}

function checkAuth() {
  const token = getAuthToken();
  if (!token) {
    window.location.href = "index.html";
    return false;
  }
  return true;
}

// Login functionality
if (document.getElementById("loginForm")) {
  document.getElementById("loginForm").addEventListener("submit", async (e) => {
    e.preventDefault();

    const email = document.getElementById("email").value;
    const password = document.getElementById("password").value;

    try {
      const response = await fetch(`${API_BASE_URL}/Auth/login`, {
        method: "POST",
        headers: {
          "Access-Control-Allow-Origin": "*",
          "Content-Type": "application/json",
        },
        body: JSON.stringify({ email, password }),
      });

      const data = await response.json();

      if (response.ok) {
        setAuthToken(data.token);
        showMessage("Đăng nhập thành công!");
        setTimeout(() => {
          window.location.href = "dashboard.html";
        }, 1000);
      } else {
        showMessage(data.title || "Đăng nhập thất bại", "error");
      }
    } catch (error) {
      console.error("Login error:", error);
      showMessage("Có lỗi xảy ra khi đăng nhập", "error");
    }
  });
}

// Dashboard functionality
if (document.getElementById("employeeTable")) {
  let employees = [];
  let currentEmployeeId = null;

  // Load employees
  async function loadEmployees() {
    if (!checkAuth()) return;

    const tbody = document.getElementById("employeeTableBody");
    tbody.innerHTML =
      '<tr><td colspan="9" class="loading">Đang tải dữ liệu...</td></tr>';

    try {
      const response = await fetch(`${API_BASE_URL}/employees`, {
        headers: getAuthHeaders(),
      });

      if (response.status === 401) {
        removeAuthToken();
        window.location.href = "index.html";
        return;
      }

      const data = await response.json();

      if (response.ok) {
        employees = data;
        renderEmployees(data);
      } else {
        tbody.innerHTML = `<tr><td colspan="9" class="loading">Lỗi: ${
          data.title || "Không thể tải dữ liệu"
        }</td></tr>`;
      }
    } catch (error) {
      console.error("Load employees error:", error);
      tbody.innerHTML =
        '<tr><td colspan="9" class="loading">Lỗi kết nối</td></tr>';
    }
  }

  function renderEmployees(employeeList) {
    const tbody = document.getElementById("employeeTableBody");

    if (employeeList.length === 0) {
      tbody.innerHTML =
        '<tr><td colspan="9" class="loading">Không có dữ liệu nhân viên</td></tr>';
      return;
    }

    tbody.innerHTML = employeeList
      .map(
        (employee) => `
            <tr>
                <td>${employee.id}</td>
                <td>${employee.fullName}</td>
                <td>${employee.email || ""}</td>
                <td>${employee.phone || ""}</td>
                <td>${employee.position || ""}</td>
                <td>${
                  employee.salary
                    ? employee.salary.toLocaleString("vi-VN") + " VND"
                    : ""
                }</td>
                <td>${
                  employee.hireDate
                    ? new Date(employee.hireDate).toLocaleDateString("vi-VN")
                    : ""
                }</td>
                <td>${employee.isActive ? "Hoạt động" : "Không hoạt động"}</td>
                <td>
                    <button class="btn btn-secondary" onclick="editEmployee(${
                      employee.id
                    })">Sửa</button>
                    <button class="btn btn-danger" onclick="deleteEmployee(${
                      employee.id
                    })">Xóa</button>
                </td>
            </tr>
        `
      )
      .join("");
  }

  // Modal functions
  function openModal(title, employee = null) {
    document.getElementById("modalTitle").textContent = title;
    document.getElementById("employeeModal").style.display = "block";

    if (employee) {
      currentEmployeeId = employee.id;
      document.getElementById("firstName").value = employee.firstName || "";
      document.getElementById("lastName").value = employee.lastName || "";
      document.getElementById("email").value = employee.email || "";
      document.getElementById("phone").value = employee.phone || "";
      document.getElementById("position").value = employee.position || "";
      document.getElementById("salary").value = employee.salary || "";
      document.getElementById("hireDate").value = employee.hireDate
        ? employee.hireDate.split("T")[0]
        : "";
      document.getElementById("isActive").checked = employee.isActive;
    } else {
      currentEmployeeId = null;
      document.getElementById("employeeForm").reset();
      document.getElementById("isActive").checked = true;
    }
  }

  function closeModal() {
    document.getElementById("employeeModal").style.display = "none";
    currentEmployeeId = null;
  }

  // Event listeners
  document.getElementById("addEmployeeBtn").addEventListener("click", () => {
    openModal("Thêm nhân viên");
  });

  document
    .getElementById("refreshBtn")
    .addEventListener("click", loadEmployees);

  document.querySelector(".close").addEventListener("click", closeModal);

  document.getElementById("cancelBtn").addEventListener("click", closeModal);

  document.getElementById("saveBtn").addEventListener("click", async () => {
    const formData = new FormData(document.getElementById("employeeForm"));
    const employeeData = {
      firstName: formData.get("firstName"),
      lastName: formData.get("lastName"),
      email: formData.get("email") || null,
      phone: formData.get("phone") || null,
      position: formData.get("position") || null,
      salary: formData.get("salary")
        ? parseFloat(formData.get("salary"))
        : null,
      hireDate: formData.get("hireDate") || null,
      isActive: document.getElementById("isActive").checked,
    };

    try {
      let response;
      if (currentEmployeeId) {
        // Update employee
        employeeData.id = currentEmployeeId;
        response = await fetch(
          `${API_BASE_URL}/employees/${currentEmployeeId}`,
          {
            method: "PUT",
            headers: {
              "Content-Type": "application/json",
              ...getAuthHeaders(),
            },
            body: JSON.stringify(employeeData),
          }
        );
      } else {
        // Create employee
        response = await fetch(`${API_BASE_URL}/employees`, {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
            ...getAuthHeaders(),
          },
          body: JSON.stringify(employeeData),
        });
      }

      if (response.ok) {
        closeModal();
        loadEmployees();
        showMessage(
          currentEmployeeId
            ? "Cập nhật nhân viên thành công!"
            : "Thêm nhân viên thành công!"
        );
      } else {
        const error = await response.json();
        showMessage(error.title || "Có lỗi xảy ra", "error");
      }
    } catch (error) {
      console.error("Save employee error:", error);
      showMessage("Có lỗi xảy ra khi lưu dữ liệu", "error");
    }
  });

  // Global functions for buttons
  window.editEmployee = async (id) => {
    try {
      const response = await fetch(`${API_BASE_URL}/employees/${id}`, {
        headers: getAuthHeaders(),
      });

      if (response.ok) {
        const employee = await response.json();
        openModal("Sửa nhân viên", employee);
      } else {
        showMessage("Không thể tải thông tin nhân viên", "error");
      }
    } catch (error) {
      console.error("Edit employee error:", error);
      showMessage("Có lỗi xảy ra", "error");
    }
  };

  window.deleteEmployee = async (id) => {
    if (!confirm("Bạn có chắc chắn muốn xóa nhân viên này?")) {
      return;
    }

    try {
      const response = await fetch(`${API_BASE_URL}/employees/${id}`, {
        method: "DELETE",
        headers: getAuthHeaders(),
      });

      if (response.ok) {
        loadEmployees();
        showMessage("Xóa nhân viên thành công!");
      } else {
        showMessage("Không thể xóa nhân viên", "error");
      }
    } catch (error) {
      console.error("Delete employee error:", error);
      showMessage("Có lỗi xảy ra khi xóa", "error");
    }
  };

  // Logout
  document.getElementById("logoutBtn").addEventListener("click", () => {
    removeAuthToken();
    window.location.href = "index.html";
  });

  // Load current user info
  async function loadUserInfo() {
    try {
      const response = await fetch(`${API_BASE_URL}/Auth/me`, {
        headers: getAuthHeaders(),
      });

      if (response.ok) {
        const user = await response.json();
        document.getElementById("userEmail").textContent = user.email;
      }
    } catch (error) {
      console.error("Load user info error:", error);
    }
  }

  // Initialize dashboard
  loadEmployees();
  loadUserInfo();
}
