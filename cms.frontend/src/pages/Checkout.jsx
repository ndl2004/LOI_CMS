import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import axiosClient from "../api/axiosClient";

function Checkout() {
    const navigate = useNavigate();

    const [cart, setCart] = useState([]);
    const [customer, setCustomer] = useState(null);
    const [notes, setNotes] = useState("");

    useEffect(() => {
        setCart(JSON.parse(localStorage.getItem("cart")) || []);
        setCustomer(JSON.parse(localStorage.getItem("customer")));
    }, []);

    const total = cart.reduce((sum, item) => sum + item.price * item.quantity, 0);

    const handleOrder = async () => {
        if (!customer) {
            alert("Vui lòng đăng nhập trước khi đặt hàng");
            navigate("/login");
            return;
        }

        if (cart.length === 0) {
            alert("Giỏ hàng đang trống");
            return;
        }

        const orderData = {
            customerId: customer.id,
            notes: notes,
            items: cart.map((item) => ({
                productId: item.id,
                quantity: item.quantity,
            })),
        };

        try {
            await axiosClient.post("/Orders", orderData);

            alert("Đặt hàng thành công");

            localStorage.removeItem("cart");

            // Báo cho Header cập nhật lại số lượng giỏ hàng
            window.dispatchEvent(new Event("cartUpdated"));

            setCart([]);

            navigate("/orders");
        } catch {
            alert("Đặt hàng thất bại. Vui lòng kiểm tra tồn kho.");
        }
    };

    return (
        <div className="container">
            <section className="section-box" style={{ marginTop: 25 }}>
                <div className="section-head">
                    <h2>Thanh toán</h2>
                </div>

                {!customer ? (
                    <p>Bạn cần đăng nhập để đặt hàng.</p>
                ) : (
                    <div className="checkout-info">
                        <p>
                            <strong>Khách hàng:</strong> {customer.fullName}
                        </p>
                        <p>
                            <strong>Email:</strong> {customer.email}
                        </p>
                        <p>
                            <strong>SĐT:</strong> {customer.phone}
                        </p>
                        <p>
                            <strong>Địa chỉ:</strong> {customer.address}
                        </p>
                    </div>
                )}

                <h3>Sản phẩm đặt mua</h3>

                {cart.length === 0 ? (
                    <p>Không có sản phẩm trong giỏ hàng.</p>
                ) : (
                    cart.map((item) => (
                        <div className="cart-row" key={item.id}>
                            <strong>{item.name}</strong>
                            <span>x{item.quantity}</span>
                            <span>{(item.price * item.quantity).toLocaleString("vi-VN")} đ</span>
                        </div>
                    ))
                )}

                <textarea
                    className="checkout-note"
                    placeholder="Ghi chú giao hàng..."
                    value={notes}
                    onChange={(e) => setNotes(e.target.value)}
                />

                <div className="cart-total">
                    Tổng tiền: <strong>{total.toLocaleString("vi-VN")} đ</strong>
                </div>

                <button className="btn-add-cart" onClick={handleOrder}>
                    Đặt hàng
                </button>
            </section>
        </div>
    );
}

export default Checkout;