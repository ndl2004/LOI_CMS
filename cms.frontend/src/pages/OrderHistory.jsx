import { useEffect, useState } from "react";
import axiosClient from "../api/axiosClient";

function OrderHistory() {
    const [orders, setOrders] = useState([]);
    const customer = JSON.parse(localStorage.getItem("customer"));

    useEffect(() => {
        if (customer) {
            axiosClient
                .get(`/Orders/customer/${customer.id}`)
                .then((res) => setOrders(res.data));
        }
    }, []);

    if (!customer) {
        return (
            <div className="container">
                <section className="section-box" style={{ marginTop: 25 }}>
                    Vui lòng đăng nhập để xem lịch sử mua hàng.
                </section>
            </div>
        );
    }

    return (
        <div className="container">
            <section className="section-box" style={{ marginTop: 25 }}>
                <div className="section-head">
                    <h2>Lịch sử mua hàng</h2>
                </div>

                {orders.length === 0 ? (
                    <p>Chưa có đơn hàng nào.</p>
                ) : (
                    orders.map((order) => (
                        <div className="order-box" key={order.id}>
                            <h3>Đơn hàng #{order.id}</h3>
                            <p>Ngày đặt: {new Date(order.orderDate).toLocaleString("vi-VN")}</p>
                            <p>Trạng thái: {order.status === 0 ? "Chờ duyệt" : "Đã xử lý"}</p>
                            <p>Ghi chú: {order.notes}</p>

                            {order.details?.map((item) => (
                                <div className="cart-row" key={item.id}>
                                    <span>{item.productName}</span>
                                    <span>x{item.quantity}</span>
                                    <span>{Number(item.totalPrice).toLocaleString("vi-VN")} đ</span>
                                </div>
                            ))}

                            <div className="cart-total">
                                Tổng tiền:{" "}
                                <strong>{Number(order.totalAmount).toLocaleString("vi-VN")} đ</strong>
                            </div>
                        </div>
                    ))
                )}
            </section>
        </div>
    );
}

export default OrderHistory;