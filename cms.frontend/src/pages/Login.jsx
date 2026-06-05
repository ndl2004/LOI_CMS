import { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import axiosClient from "../api/axiosClient";

function Login() {
    const navigate = useNavigate();

    const [form, setForm] = useState({
        email: "",
        password: "",
    });

    const handleChange = (e) => {
        setForm({ ...form, [e.target.name]: e.target.value });
    };

    const handleSubmit = async (e) => {
        e.preventDefault();

        try {
            const res = await axiosClient.post("/Auth/CustomerLogin", form);
            localStorage.setItem("customer", JSON.stringify(res.data));

            window.dispatchEvent(new Event("customerUpdated"));

            alert("Đăng nhập thành công");
            navigate("/checkout");
        } catch {
            alert("Email hoặc mật khẩu không đúng");
        }
    };

    return (
        <div className="container">
            <section className="auth-box">
                <h2>Đăng nhập</h2>

                <form onSubmit={handleSubmit}>
                    <input name="email" placeholder="Email" onChange={handleChange} required />
                    <input name="password" type="password" placeholder="Mật khẩu" onChange={handleChange} required />

                    <button type="submit">Đăng nhập</button>
                </form>

                <p>
                    Chưa có tài khoản? <Link to="/register">Đăng ký</Link>
                </p>
            </section>
        </div>
    );
}

export default Login;