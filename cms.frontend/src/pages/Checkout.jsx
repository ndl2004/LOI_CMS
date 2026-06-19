import { useEffect, useState } from "react";
import { useNavigate, Link } from "react-router-dom";
import axiosClient from "../api/axiosClient";
import { IMAGE_BASE_URL } from "../config";

function Checkout() {
    const navigate = useNavigate();

    const [cart, setCart] = useState([]);
    const [customer, setCustomer] = useState(null);
    const [notes, setNotes] = useState("");
    const [loading, setLoading] = useState(false);

    const [fullName, setFullName] = useState("");
    const [email, setEmail] = useState("");
    const [phone, setPhone] = useState("");
    const [address, setAddress] = useState("");

    useEffect(() => {
        const cartData = JSON.parse(localStorage.getItem("cart")) || [];
        const customerData = JSON.parse(localStorage.getItem("customer"));

        setCart(cartData);
        setCustomer(customerData);

        if (customerData) {
            setFullName(customerData.fullName || "");
            setEmail(customerData.email || "");
            setPhone(customerData.phone || "");
            setAddress(customerData.address || "");
        }
    }, []);

    const total = cart.reduce(
        (sum, item) => sum + Number(item.price) * item.quantity,
        0
    );

    const shippingFee = 0;
    const finalTotal = total + shippingFee;

    const validateForm = () => {
        if (!fullName.trim()) {
            alert("Vui lòng nhập họ và tên người nhận");
            return false;
        }

        if (!email.trim()) {
            alert("Vui lòng nhập email nhận thông tin đơn hàng");
            return false;
        }

        if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email.trim())) {
            alert("Email nhận đơn hàng không hợp lệ");
            return false;
        }

        if (!phone.trim()) {
            alert("Vui lòng nhập số điện thoại");
            return false;
        }

        if (!/^0\d{9,10}$/.test(phone.trim())) {
            alert("Số điện thoại không hợp lệ");
            return false;
        }

        if (!address.trim()) {
            alert("Vui lòng nhập địa chỉ giao hàng");
            return false;
        }

        return true;
    };

    const handleOrder = async () => {
        if (!customer) {
            alert("Vui lòng đăng nhập trước khi đặt hàng");
            localStorage.setItem("redirectAfterLogin", "/checkout");
            navigate("/login", {
                state: {
                    from: "/checkout",
                },
            });
            return;
        }

        if (cart.length === 0) {
            alert("Giỏ hàng đang trống");
            navigate("/shop");
            return;
        }

        if (!validateForm()) return;

        const orderData = {
            customerId: customer.id,
            fullName: fullName.trim(),
            email: email.trim(),
            phone: phone.trim(),
            address: address.trim(),
            notes: notes,
            items: cart.map((item) => ({
                productId: item.id,
                quantity: item.quantity,
            })),
        };

        try {
            setLoading(true);

            await axiosClient.post("/Orders", orderData);

            alert("Đặt hàng thành công");

            localStorage.removeItem("cart");
            window.dispatchEvent(new Event("cartUpdated"));

            setCart([]);
            navigate("/orders");
        } catch (err) {
            console.error("Lỗi đặt hàng:", err);
            alert(
                err.response?.data?.message ||
                "Đặt hàng thất bại. Vui lòng kiểm tra tồn kho hoặc thông tin đơn hàng."
            );
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="container">
            <section className="checkout-page">
                <div className="checkout-header">
                    <div>
                        <h2>Thanh toán</h2>
                        <p>Nhập đầy đủ thông tin giao hàng và email nhận đơn hàng</p>
                    </div>

                    <Link to="/cart" className="cart-back-link">
                        ← Quay lại giỏ hàng
                    </Link>
                </div>

                {!customer ? (
                    <div className="checkout-login-box">
                        <h3>Bạn cần đăng nhập để đặt hàng</h3>
                        <p>Vui lòng đăng nhập tài khoản khách hàng trước khi thanh toán.</p>

                        <Link
                            to="/login"
                            state={{ from: "/checkout" }}
                            className="checkout-button"
                            onClick={() =>
                                localStorage.setItem(
                                    "redirectAfterLogin",
                                    "/checkout"
                                )
                            }
                        >
                            Đăng nhập ngay
                        </Link>
                    </div>
                ) : cart.length === 0 ? (
                    <div className="empty-cart">
                        <div className="empty-icon">🛒</div>
                        <h3>Không có sản phẩm trong giỏ hàng</h3>
                        <p>Hãy chọn thêm sản phẩm trước khi thanh toán.</p>

                        <Link to="/shop" className="btn-add-cart">
                            Tiếp tục mua hàng
                        </Link>
                    </div>
                ) : (
                    <div className="checkout-layout">
                        <div className="checkout-left">
                            <div className="checkout-card">
                                <h3>Thông tin giao hàng</h3>

                                <div className="checkout-form">
                                    <div className="form-group">
                                        <label>Họ và tên người nhận *</label>
                                        <input
                                            type="text"
                                            value={fullName}
                                            onChange={(e) => setFullName(e.target.value)}
                                            placeholder="Nhập họ và tên"
                                            required
                                        />
                                    </div>

                                    <div className="form-group">
                                        <label>Email nhận đơn hàng *</label>
                                        <input
                                            type="email"
                                            value={email}
                                            onChange={(e) => setEmail(e.target.value)}
                                            placeholder="Nhập email nhận xác nhận đơn hàng"
                                            required
                                        />
                                    </div>

                                    <div className="form-group">
                                        <label>Số điện thoại *</label>
                                        <input
                                            type="text"
                                            value={phone}
                                            onChange={(e) => setPhone(e.target.value)}
                                            placeholder="Ví dụ: 0901234567"
                                            required
                                        />
                                    </div>

                                    <div className="form-group">
                                        <label>Địa chỉ giao hàng *</label>
                                        <textarea
                                            value={address}
                                            onChange={(e) => setAddress(e.target.value)}
                                            placeholder="Nhập địa chỉ nhận hàng"
                                            required
                                        />
                                    </div>
                                </div>
                            </div>

                            <div className="checkout-card">
                                <h3>Sản phẩm đặt mua</h3>

                                <div className="checkout-products">
                                    {cart.map((item) => {
                                        const imageUrl = item.imageUrl
                                            ? `${IMAGE_BASE_URL}${item.imageUrl}`
                                            : "https://via.placeholder.com/300x300?text=LOI+Cosmetics";

                                        const itemTotal = Number(item.price) * item.quantity;

                                        return (
                                            <div className="checkout-product" key={item.id}>
                                                <img src={imageUrl} alt={item.name} />

                                                <div className="checkout-product-info">
                                                    <h4>{item.name}</h4>
                                                    <p>
                                                        {item.isFlashDeal && item.originalPrice && (
                                                            <span className="cart-old-price">
                                                                {Number(item.originalPrice).toLocaleString("vi-VN")} đ
                                                            </span>
                                                        )}
                                                        {Number(item.price).toLocaleString("vi-VN")} đ
                                                    </p>
                                                    {item.isFlashDeal && (
                                                        <span className="cart-sale-note">
                                                            Flash Sale -{item.discountPercent}%
                                                        </span>
                                                    )}
                                                    <span>Số lượng: x{item.quantity}</span>
                                                </div>

                                                <strong>
                                                    {itemTotal.toLocaleString("vi-VN")} đ
                                                </strong>
                                            </div>
                                        );
                                    })}
                                </div>
                            </div>

                            <div className="checkout-card">
                                <h3>Ghi chú giao hàng</h3>

                                <textarea
                                    className="checkout-note"
                                    placeholder="Ví dụ: Giao giờ hành chính, gọi trước khi giao..."
                                    value={notes}
                                    onChange={(e) => setNotes(e.target.value)}
                                />
                            </div>
                        </div>

                        <div className="checkout-summary">
                            <h3>Tóm tắt thanh toán</h3>

                            <div className="summary-row">
                                <span>Tạm tính</span>
                                <strong>{total.toLocaleString("vi-VN")} đ</strong>
                            </div>

                            <div className="summary-row">
                                <span>Phí vận chuyển</span>
                                <strong>Miễn phí</strong>
                            </div>

                            <div className="summary-row">
                                <span>Phương thức</span>
                                <strong>Thanh toán khi nhận hàng</strong>
                            </div>

                            <div className="summary-total">
                                <span>Tổng thanh toán</span>
                                <strong>{finalTotal.toLocaleString("vi-VN")} đ</strong>
                            </div>

                            <button
                                className="checkout-button"
                                onClick={handleOrder}
                                disabled={loading}
                            >
                                {loading ? "Đang xử lý..." : "Xác nhận đặt hàng"}
                            </button>

                            <p className="checkout-note-small">
                                Các trường có dấu * là bắt buộc. Email có thể thay đổi để nhận thông tin đơn hàng.
                            </p>
                        </div>
                    </div>
                )}
            </section>
        </div>
    );
}

export default Checkout;
