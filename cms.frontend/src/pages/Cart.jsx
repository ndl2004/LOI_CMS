import { useEffect, useState } from "react";
import { Link } from "react-router-dom";

function Cart() {
    const [cart, setCart] = useState([]);

    useEffect(() => {
        setCart(JSON.parse(localStorage.getItem("cart")) || []);
    }, []);

    const updateQuantity = (id, quantity) => {
        if (quantity < 1) return;

        const newCart = cart.map((item) =>
            item.id === id ? { ...item, quantity } : item
        );

        setCart(newCart);
        localStorage.setItem("cart", JSON.stringify(newCart));
        window.dispatchEvent(new Event("cartUpdated"));
    };

    const removeItem = (id) => {
        const newCart = cart.filter((item) => item.id !== id);

        setCart(newCart);
        localStorage.setItem("cart", JSON.stringify(newCart));
        window.dispatchEvent(new Event("cartUpdated"));
    };

    const total = cart.reduce(
        (sum, item) => sum + Number(item.price) * item.quantity,
        0
    );

    return (
        <div className="container">
            <section className="cart-page">
                <div className="cart-header">
                    <div>
                        <h2>Giỏ hàng của bạn</h2>
                        <p>Kiểm tra sản phẩm trước khi thanh toán</p>
                    </div>

                    <Link to="/shop" className="cart-back-link">
                        ← Tiếp tục mua hàng
                    </Link>
                </div>

                {cart.length === 0 ? (
                    <div className="empty-cart">
                        <div className="empty-icon">🛒</div>
                        <h3>Giỏ hàng đang trống</h3>
                        <p>Hãy chọn thêm sản phẩm yêu thích của bạn.</p>

                        <Link to="/shop" className="btn-add-cart">
                            Mua sắm ngay
                        </Link>
                    </div>
                ) : (
                    <div className="cart-layout">
                        <div className="cart-list">
                            {cart.map((item) => {
                                const imageUrl = item.imageUrl
                                    ? `https://localhost:7175${item.imageUrl}`
                                    : "https://via.placeholder.com/300x300?text=LOI+Cosmetics";

                                const itemTotal =
                                    Number(item.price) * item.quantity;

                                return (
                                    <div className="cart-item" key={item.id}>
                                        <div className="cart-product">
                                            <img src={imageUrl} alt={item.name} />

                                            <div>
                                                <h3>{item.name}</h3>

                                                <p className="cart-price">
                                                    {Number(item.price).toLocaleString(
                                                        "vi-VN"
                                                    )}{" "}
                                                    đ
                                                </p>

                                                <button
                                                    className="cart-remove-mobile"
                                                    onClick={() => removeItem(item.id)}
                                                >
                                                    Xóa sản phẩm
                                                </button>
                                            </div>
                                        </div>

                                        <div className="cart-quantity">
                                            <button
                                                onClick={() =>
                                                    updateQuantity(
                                                        item.id,
                                                        item.quantity - 1
                                                    )
                                                }
                                            >
                                                -
                                            </button>

                                            <input value={item.quantity} readOnly />

                                            <button
                                                onClick={() =>
                                                    updateQuantity(
                                                        item.id,
                                                        item.quantity + 1
                                                    )
                                                }
                                            >
                                                +
                                            </button>
                                        </div>

                                        <div className="cart-item-total">
                                            {itemTotal.toLocaleString("vi-VN")} đ
                                        </div>

                                        <button
                                            className="btn-remove"
                                            onClick={() => removeItem(item.id)}
                                        >
                                            Xóa
                                        </button>
                                    </div>
                                );
                            })}
                        </div>

                        <div className="cart-summary">
                            <h3>Tóm tắt đơn hàng</h3>

                            <div className="summary-row">
                                <span>Số sản phẩm</span>
                                <strong>{cart.length}</strong>
                            </div>

                            <div className="summary-row">
                                <span>Tạm tính</span>
                                <strong>{total.toLocaleString("vi-VN")} đ</strong>
                            </div>

                            <div className="summary-row">
                                <span>Phí vận chuyển</span>
                                <strong>Miễn phí</strong>
                            </div>

                            <div className="summary-total">
                                <span>Tổng thanh toán</span>
                                <strong>{total.toLocaleString("vi-VN")} đ</strong>
                            </div>

                            <Link className="checkout-button" to="/checkout">
                                Tiến hành thanh toán
                            </Link>
                        </div>
                    </div>
                )}
            </section>
        </div>
    );
}

export default Cart;