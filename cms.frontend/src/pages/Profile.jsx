import { useEffect, useState } from "react";
import axiosClient from "../api/axiosClient";

function Profile() {
    const [form, setForm] = useState({
        id: 0,
        fullName: "",
        email: "",
        phone: "",
        address: "",
    });

    const [loading, setLoading] = useState(false);

    useEffect(() => {
        const customer =
            JSON.parse(localStorage.getItem("customer"));

        if (customer) {
            setForm({
                id: customer.id,
                fullName: customer.fullName || "",
                email: customer.email || "",
                phone: customer.phone || "",
                address: customer.address || "",
            });
        }
    }, []);

    const handleChange = (e) => {
        setForm({
            ...form,
            [e.target.name]: e.target.value,
        });
    };

    const handleSubmit = async (e) => {
        e.preventDefault();

        try {
            setLoading(true);

            const res = await axiosClient.post(
                "/Auth/UpdateProfile",
                form
            );

            localStorage.setItem(
                "customer",
                JSON.stringify(res.data)
            );

            window.dispatchEvent(
                new Event("customerUpdated")
            );

            alert("Cập nhật hồ sơ thành công");
        }
        catch (err) {
            alert(
                err.response?.data?.message ||
                "Cập nhật thất bại"
            );
        }
        finally {
            setLoading(false);
        }
    };

    return (
        <div className="container">
            <section className="auth-box">

                <h2>Hồ sơ cá nhân</h2>

                <form onSubmit={handleSubmit}>

                    <input
                        name="fullName"
                        placeholder="Họ và tên"
                        value={form.fullName}
                        onChange={handleChange}
                        required
                    />

                    <input
                        name="email"
                        type="email"
                        placeholder="Email"
                        value={form.email}
                        onChange={handleChange}
                        required
                    />

                    <input
                        name="phone"
                        placeholder="Số điện thoại"
                        value={form.phone}
                        onChange={handleChange}
                    />

                    <input
                        name="address"
                        placeholder="Địa chỉ"
                        value={form.address}
                        onChange={handleChange}
                    />

                    <button
                        type="submit"
                        disabled={loading}
                    >
                        {
                            loading
                                ? "Đang cập nhật..."
                                : "Cập nhật hồ sơ"
                        }
                    </button>

                </form>

            </section>
        </div>
    );
}

export default Profile;