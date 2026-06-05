import { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import axiosClient from "../api/axiosClient";

function Register() {
    const navigate = useNavigate();

    const [form, setForm] = useState({
        fullName: "",
        email: "",
        password: "",
        phone: "",
        address: "",
    });

    const handleChange = (e) => {
        setForm({ ...form, [e.target.name]: e.target.value });
    };

    const handleSubmit = async (e) => {
        e.preventDefault();

        try {
            await axiosClient.post("/Auth/CustomerRegister", form);
            alert("Đăng ký thành công");
            navigate("/login");
        } catch {
            alert("Đăng ký thất bại. Email có thể đã tồn tại.");
        }
    };

    return (
        <div className="container">
            <section className="auth-box">
                <h2>Đăng ký tài khoản</h2>

                <form onSubmit={handleSubmit}>
                    <input name="fullName" placeholder="Họ tên" onChange={handleChange} required />
                    <input name="email" placeholder="Email" onChange={handleChange} required />
                    <input name="password" type="password" placeholder="Mật khẩu" onChange={handleChange} required />
                    <input name="phone" placeholder="Số điện thoại" onChange={handleChange} />
                    <input name="address" placeholder="Địa chỉ" onChange={handleChange} />

                    <button type="submit">Đăng ký</button>
                </form>

                <p>
                    Đã có tài khoản? <Link to="/login">Đăng nhập</Link>
                </p>
            </section>
        </div>
    );
}

export default Register;