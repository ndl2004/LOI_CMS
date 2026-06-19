import { useEffect, useState } from "react";
import axiosClient from "../api/axiosClient";

function OrderHistory() {
    const [orders, setOrders] = useState([]);
    const customer = JSON.parse(localStorage.getItem("customer"));

    useEffect(() => {
        const customerData = JSON.parse(localStorage.getItem("customer"));

        if (customerData) {
            axiosClient
                .get(`/Orders/customer/${customerData.id}`)
                .then((res) => setOrders(res.data));
        }
    }, []);

    const getStatusText = (status) => {
        switch (status) {
            case 0:
                return "Chờ duyệt";
            case 1:
                return "Đã duyệt / Đang giao";
            case 2:
                return "Hoàn thành";
            case 3:
                return "Từ chối";
            default:
                return "Không xác định";
        }
    };

    const getStatusMessage = (status) => {
        switch (status) {
            case 0:
                return "Đơn hàng của bạn đang chờ admin xác nhận.";
            case 1:
                return "Đơn hàng đã được duyệt và đang được xử lý giao hàng.";
            case 2:
                return "Đơn hàng đã hoàn thành. Cảm ơn bạn đã mua hàng.";
            case 3:
                return "Đơn hàng đã bị từ chối. Sản phẩm trong đơn đã được hoàn lại tồn kho.";
            default:
                return "";
        }
    };

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
                    <div className="order-history-list">
                        {orders.map((order) => {
                            const subTotal = order.details?.reduce(
                                (sum, item) => sum + Number(item.totalPrice),
                                0
                            ) || 0;

                            return (
                                <div className="order-box" key={order.id}>
                                    <div className="order-title-row">
                                        <div>
                                            <h3>Đơn hàng #{order.id}</h3>
                                            <p>
                                                Ngày đặt:{" "}
                                                {new Date(order.orderDate).toLocaleString("vi-VN")}
                                            </p>
                                        </div>

                                        <span className={`order-status status-${order.status}`}>
                                            {getStatusText(order.status)}
                                        </span>
                                    </div>

                                    <div className={`order-status-message status-message-${order.status}`}>
                                        {getStatusMessage(order.status)}
                                    </div>

                                    <div className="order-shipping-info">
                                        <p><strong>Người nhận:</strong> {order.receiverName || customer.fullName}</p>
                                        <p><strong>Số điện thoại:</strong> {order.receiverPhone || customer.phone}</p>
                                        <p><strong>Địa chỉ:</strong> {order.shippingAddress || customer.address}</p>
                                        <p><strong>Phương thức:</strong> {order.paymentMethod || "COD"}</p>
                                        {order.notes && <p><strong>Ghi chú:</strong> {order.notes}</p>}
                                    </div>

                                    <div className="order-detail-list">
                                        {order.details?.map((item) => (
                                            <div className="order-detail-row" key={item.id}>
                                                <div>
                                                    <strong>{item.productName}</strong>
                                                    {item.isFlashSale && (
                                                        <span className="cart-sale-note">
                                                            {item.discountPercent > 0
                                                                ? `Flash Sale -${item.discountPercent}%`
                                                                : "Giá ưu đãi"}
                                                        </span>
                                                    )}
                                                </div>

                                                <span>x{item.quantity}</span>

                                                <div className="order-price-column">
                                                    {item.isFlashSale && item.originalPrice > item.unitPrice && (
                                                        <span className="cart-old-price">
                                                            {Number(item.originalPrice).toLocaleString("vi-VN")} đ
                                                        </span>
                                                    )}
                                                    <strong>
                                                        {Number(item.unitPrice).toLocaleString("vi-VN")} đ
                                                    </strong>
                                                </div>

                                                <strong>
                                                    {Number(item.totalPrice).toLocaleString("vi-VN")} đ
                                                </strong>
                                            </div>
                                        ))}
                                    </div>

                                    <div className="order-total-box">
                                        <div>
                                            <span>Tạm tính</span>
                                            <strong>{subTotal.toLocaleString("vi-VN")} đ</strong>
                                        </div>
                                        <div>
                                            <span>Phí vận chuyển</span>
                                            <strong>{Number(order.shippingFee || 0).toLocaleString("vi-VN")} đ</strong>
                                        </div>
                                        <div className="order-grand-total">
                                            <span>Tổng tiền</span>
                                            <strong>{Number(order.totalAmount).toLocaleString("vi-VN")} đ</strong>
                                        </div>
                                    </div>
                                </div>
                            );
                        })}
                    </div>
                )}
            </section>
        </div>
    );
}

export default OrderHistory;
