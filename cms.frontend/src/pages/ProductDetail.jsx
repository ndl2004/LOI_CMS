import { useEffect, useState } from "react";
import { useParams, Link } from "react-router-dom";
import axiosClient from "../api/axiosClient";

function ProductDetail() {
    const { id } = useParams();

    const [product, setProduct] = useState(null);
    const [quantity, setQuantity] = useState(1);

    useEffect(() => {
        axiosClient
            .get(`/Products/${id}`)
            .then((res) => setProduct(res.data))
            .catch((err) => console.log("Lỗi Product Detail:", err));
    }, [id]);

    if (!product) {
        return (
            <div className="container">
                <div className="section-box" style={{ marginTop: 30 }}>
                    Đang tải sản phẩm...
                </div>
            </div>
        );
    }

    const imageUrl = product.imageUrl
        ? `https://localhost:7175${product.imageUrl}`
        : "https://via.placeholder.com/500x500?text=LOI+Cosmetics";

    const price = Number(product.price);
    const totalPrice = price * quantity;
    const stockQuantity = product.stockQuantity || 0;
    const soldQuantity = product.soldQuantity || 0;

    const increaseQuantity = () => {
        if (quantity < stockQuantity) {
            setQuantity(quantity + 1);
        } else {
            alert("Số lượng mua không được vượt quá tồn kho");
        }
    };

    const decreaseQuantity = () => {
        if (quantity > 1) {
            setQuantity(quantity - 1);
        }
    };

    const addToCart = () => {
        if (stockQuantity <= 0) {
            alert("Sản phẩm đã hết hàng");
            return;
        }

        const cart = JSON.parse(localStorage.getItem("cart")) || [];

        const existingItem = cart.find((item) => item.id === product.id);

        if (existingItem) {
            const newQuantity = existingItem.quantity + quantity;

            if (newQuantity > stockQuantity) {
                alert("Số lượng trong giỏ hàng vượt quá tồn kho");
                return;
            }

            existingItem.quantity = newQuantity;
        } else {
            cart.push({
                id: product.id,
                name: product.name,
                price: product.price,
                imageUrl: product.imageUrl,
                quantity: quantity,
            });
        }

        localStorage.setItem("cart", JSON.stringify(cart));
        window.dispatchEvent(new Event("cartUpdated"));

        alert("Đã thêm sản phẩm vào giỏ hàng");
    };

    return (
        <div className="container">
            <section className="detail-box">
                <div className="detail-image">
                    <img src={imageUrl} alt={product.name} />
                </div>

                <div className="detail-info">
                    <span className="detail-category">
                        {product.categoryName || "Mỹ phẩm"}
                    </span>

                    <h1>{product.name}</h1>

                    <div className="detail-price">
                        {price.toLocaleString("vi-VN")} đ
                    </div>

                    <div className="detail-meta">
                        <span>
                            Đã bán: <strong>{soldQuantity}</strong>
                        </span>

                        <span>
                            Tồn kho: <strong>{stockQuantity}</strong>
                        </span>
                    </div>

                    <p className="detail-desc">
                        {product.description ||
                            "Sản phẩm chăm sóc sắc đẹp chất lượng cao, phù hợp sử dụng hằng ngày."}
                    </p>

                    <div className="detail-benefits">
                        <div>✅ Cam kết chính hãng</div>
                        <div>✅ Đổi trả nếu sản phẩm lỗi</div>
                        <div>✅ Tư vấn chăm sóc da miễn phí</div>
                        <div>✅ Giao hàng nhanh trong khu vực nội thành</div>
                    </div>

                    <div className="quantity-section">
                        <span>Số lượng</span>

                        <div className="quantity-box">
                            <button onClick={decreaseQuantity}>-</button>

                            <input value={quantity} readOnly />

                            <button onClick={increaseQuantity}>+</button>
                        </div>
                    </div>

                    <div className="total-price-box">
                        <span>Tạm tính:</span>
                        <strong>{totalPrice.toLocaleString("vi-VN")} đ</strong>
                    </div>

                    <div className="detail-actions">
                        <button className="btn-add-cart" onClick={addToCart}>
                            🛒 Thêm vào giỏ hàng
                        </button>

                        <Link to="/shop" className="btn-back-shop">
                            ← Quay lại cửa hàng
                        </Link>
                    </div>
                </div>
            </section>
        </div>
    );
}

export default ProductDetail;