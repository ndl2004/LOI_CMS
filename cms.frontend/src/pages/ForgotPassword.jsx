import { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import axiosClient from "../api/axiosClient";

function ForgotPassword() {
    const navigate = useNavigate();

    const [step, setStep] = useState(1);
    const [email, setEmail] = useState("");
    const [otp, setOtp] = useState("");
    const [newPassword, setNewPassword] = useState("");
    const [confirmPassword, setConfirmPassword] = useState("");
    const [loading, setLoading] = useState(false);
    const [showPassword, setShowPassword] = useState(false);
    const [showConfirmPassword, setShowConfirmPassword] = useState(false);
    const sendOtp = async (e) => {
        e.preventDefault();

        if (!email.trim()) {
            alert("Vui lòng nhập email");
            return;
        }

        try {
            setLoading(true);

            await axiosClient.post("/Auth/SendForgotPasswordOtp", {
                email: email.trim(),
            });

            alert("Mã OTP đã được gửi về email của bạn");
            setStep(2);
        } catch (err) {
            alert(err.response?.data?.message || "Gửi OTP thất bại");
        } finally {
            setLoading(false);
        }
    };

    const resetPassword = async (e) => {
        e.preventDefault();

        if (!otp.trim()) {
            alert("Vui lòng nhập mã OTP");
            return;
        }

        if (newPassword.length < 6) {
            alert("Mật khẩu mới phải từ 6 ký tự trở lên");
            return;
        }

        if (newPassword !== confirmPassword) {
            alert("Mật khẩu xác nhận không khớp");
            return;
        }

        try {
            setLoading(true);

            await axiosClient.post("/Auth/ResetPasswordWithOtp", {
                email: email.trim(),
                otp: otp.trim(),
                newPassword: newPassword,
            });

            alert("Đặt lại mật khẩu thành công");
            navigate("/login");
        } catch (err) {
            alert(err.response?.data?.message || "Đặt lại mật khẩu thất bại");
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="container">
            <section className="auth-box">
                <h2>Quên mật khẩu</h2>

                {step === 1 ? (
                    <form onSubmit={sendOtp}>
                        <input
                            type="email"
                            placeholder="Nhập email tài khoản"
                            value={email}
                            onChange={(e) => setEmail(e.target.value)}
                            required
                        />

                        <button type="submit" disabled={loading}>
                            {loading ? "Đang gửi OTP..." : "Gửi mã OTP"}
                        </button>
                    </form>
                ) : (
                    <form onSubmit={resetPassword}>
                        <input
                            type="text"
                            value={email}
                            readOnly
                        />

                        <input
                            type="text"
                            placeholder="Nhập mã OTP"
                            value={otp}
                            onChange={(e) => setOtp(e.target.value)}
                            required
                        />

                            <div className="password-wrapper">
                                <input
                                    type={showPassword ? "text" : "password"}
                                    placeholder="Mật khẩu mới"
                                    value={newPassword}
                                    onChange={(e) => setNewPassword(e.target.value)}
                                    required
                                />

                                <span
                                    className="password-eye"
                                    onClick={() =>
                                        setShowPassword(!showPassword)
                                    }
                                >
                                    {showPassword ? "🙈" : "👁"}
                                </span>
                            </div>

                            <div className="password-wrapper">
                                <input
                                    type={
                                        showConfirmPassword
                                            ? "text"
                                            : "password"
                                    }
                                    placeholder="Nhập lại mật khẩu mới"
                                    value={confirmPassword}
                                    onChange={(e) =>
                                        setConfirmPassword(e.target.value)
                                    }
                                    required
                                />

                                <span
                                    className="password-eye"
                                    onClick={() =>
                                        setShowConfirmPassword(
                                            !showConfirmPassword
                                        )
                                    }
                                >
                                    {showConfirmPassword ? "🙈" : "👁"}
                                </span>
                            </div>

                            <div className="forgot-actions">
                                <button
                                    type="button"
                                    className="btn-secondary-auth"
                                    onClick={() => setStep(1)}
                                >
                                    Đổi email
                                </button>

                                <button
                                    type="submit"
                                    disabled={loading}
                                >
                                    {loading
                                        ? "Đang đặt lại..."
                                        : "Đặt lại mật khẩu"}
                                </button>
                            </div>
                    </form>
                )}

                <p>
                    Đã nhớ mật khẩu?
                    <Link to="/login"> Đăng nhập</Link>
                </p>
            </section>
        </div>
    );
}

export default ForgotPassword;