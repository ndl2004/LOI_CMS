import { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import axiosClient from "../api/axiosClient";

function Login() {
    const navigate = useNavigate();

    const [form, setForm] = useState({
        email: "",
        password: "",
    });

    const [loading, setLoading] = useState(false);

    const handleChange = (e) => {
        setForm({
            ...form,
            [e.target.name]: e.target.value,
        });
    };

    const handleSubmit = async (e) => {
        e.preventDefault();

        if (!form.email.trim()) {
            alert("Vui lòng nhập Email");
            return;
        }

        if (!form.password.trim()) {
            alert("Vui lòng nhập mật khẩu");
            return;
        }

        try {
            setLoading(true);

            const res = await axiosClient.post(
                "/Auth/CustomerLogin",
                form
            );

            localStorage.setItem(
                "customer",
                JSON.stringify(res.data)
            );

            window.dispatchEvent(
                new Event("customerUpdated")
            );

            alert("Đăng nhập thành công");

            navigate("/checkout");
        }
        catch (err) {
            console.error(err);

            alert("Email hoặc mật khẩu không đúng");
        }
        finally {
            setLoading(false);
        }
    };

    return (
        <div className="container">
            <section className="auth-box">

                <h2>Đăng nhập tài khoản</h2>

                <form onSubmit={handleSubmit}>

                    <input
                        name="email"
                        type="email"
                        placeholder="Email"
                        value={form.email}
                        onChange={handleChange}
                        required
                    />

                    <input
                        name="password"
                        type="password"
                        placeholder="Mật khẩu"
                        value={form.password}
                        onChange={handleChange}
                        required
                    />

                    <button
                        type="submit"
                        disabled={loading}
                    >
                        {loading
                            ? "Đang đăng nhập..."
                            : "Đăng nhập"}
                    </button>

                </form>

                <p>
                    Chưa có tài khoản?
                    <Link to="/register">
                        {" "}Đăng ký
                    </Link>
                </p>

            </section>
        </div>
    );
}

export default Login;