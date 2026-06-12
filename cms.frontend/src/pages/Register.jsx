import { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import axiosClient from "../api/axiosClient";

function Register() {
    const navigate = useNavigate();

    const [form, setForm] = useState({
        fullName: "",
        email: "",
        password: "",
        confirmPassword: "",
        phone: "",
        address: "",
    });

    const handleChange = (e) => {
        setForm({
            ...form,
            [e.target.name]: e.target.value,
        });
    };

    const handleSubmit = async (e) => {
        e.preventDefault();

        if (form.password.length < 6) {
            alert("Mật khẩu phải từ 6 ký tự trở lên");
            return;
        }

        if (form.password !== form.confirmPassword) {
            alert("Mật khẩu xác nhận không khớp");
            return;
        }

        try {
            await axiosClient.post("/Auth/CustomerRegister", {
                fullName: form.fullName,
                email: form.email,
                password: form.password,
                phone: form.phone,
                address: form.address,
            });

            alert("Đăng ký thành công");
            navigate("/login");
        }
        catch {
            alert("Email đã tồn tại hoặc đăng ký thất bại");
        }
    };

    return (
        <div className="container">
            <section className="auth-box">

                <h2>Đăng ký tài khoản</h2>

                <form onSubmit={handleSubmit}>

                    <input
                        name="fullName"
                        placeholder="Họ và tên"
                        onChange={handleChange}
                        required
                    />

                    <input
                        name="email"
                        type="email"
                        placeholder="Email"
                        onChange={handleChange}
                        required
                    />

                    <input
                        name="phone"
                        placeholder="Số điện thoại"
                        onChange={handleChange}
                        required
                    />

                    <input
                        name="address"
                        placeholder="Địa chỉ"
                        onChange={handleChange}
                        required
                    />

                    <input
                        name="password"
                        type="password"
                        placeholder="Mật khẩu"
                        minLength="6"
                        onChange={handleChange}
                        required
                    />

                    <input
                        name="confirmPassword"
                        type="password"
                        placeholder="Nhập lại mật khẩu"
                        minLength="6"
                        onChange={handleChange}
                        required
                    />

                    <button type="submit">
                        Đăng ký
                    </button>

                </form>

                <p>
                    Đã có tài khoản?
                    <Link to="/login"> Đăng nhập</Link>
                </p>

            </section>
        </div>
    );
}

export default Register;