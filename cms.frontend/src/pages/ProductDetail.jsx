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

    const addToCart = () => {
        const cart = JSON.parse(localStorage.getItem("cart")) || [];

        const existingItem = cart.find((item) => item.id === product.id);

        if (existingItem) {
            existingItem.quantity += quantity;
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
                        {Number(product.price).toLocaleString("vi-VN")} đ
                    </div>

                    <p className="detail-desc">
                        {product.description || "Sản phẩm chăm sóc sắc đẹp chất lượng cao."}
                    </p>

                    <div className="detail-stock">
                        Tồn kho: <strong>{product.stockQuantity}</strong>
                    </div>

                    <div className="quantity-box">
                        <button
                            onClick={() => quantity > 1 && setQuantity(quantity - 1)}
                        >
                            -
                        </button>

                        <input value={quantity} readOnly />

                        <button
                            onClick={() =>
                                quantity < product.stockQuantity && setQuantity(quantity + 1)
                            }
                        >
                            +
                        </button>
                    </div>

                    <button className="btn-add-cart" onClick={addToCart}>
                        🛒 Thêm vào giỏ hàng
                    </button>

                    <Link to="/shop" className="btn-back-shop">
                        ← Quay lại cửa hàng
                    </Link>
                </div>
            </section>
        </div>
    );
}

export default ProductDetail;