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
    };

    const removeItem = (id) => {
        const newCart = cart.filter((item) => item.id !== id);
        setCart(newCart);
        localStorage.setItem("cart", JSON.stringify(newCart));
    };

    const total = cart.reduce((sum, item) => sum + item.price * item.quantity, 0);

    return (
        <div className="container">
            <section className="section-box" style={{ marginTop: 25 }}>
                <div className="section-head">
                    <h2>Giỏ hàng</h2>
                </div>

                {cart.length === 0 ? (
                    <p>Giỏ hàng đang trống.</p>
                ) : (
                    <>
                        {cart.map((item) => (
                            <div className="cart-row" key={item.id}>
                                <div>
                                    <strong>{item.name}</strong>
                                    <p>{Number(item.price).toLocaleString("vi-VN")} đ</p>
                                </div>

                                <div className="quantity-box">
                                    <button onClick={() => updateQuantity(item.id, item.quantity - 1)}>-</button>
                                    <input value={item.quantity} readOnly />
                                    <button onClick={() => updateQuantity(item.id, item.quantity + 1)}>+</button>
                                </div>

                                <div>
                                    {(item.price * item.quantity).toLocaleString("vi-VN")} đ
                                </div>

                                <button className="btn-remove" onClick={() => removeItem(item.id)}>
                                    Xóa
                                </button>
                            </div>
                        ))}

                        <div className="cart-total">
                            Tổng tiền: <strong>{total.toLocaleString("vi-VN")} đ</strong>
                        </div>

                        <Link className="btn-add-cart" to="/checkout">
                            Tiến hành thanh toán
                        </Link>
                    </>
                )}
            </section>
        </div>
    );
}

export default Cart;