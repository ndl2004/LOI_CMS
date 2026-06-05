import { Link } from "react-router-dom";

function ProductCard({ product }) {
    const imageUrl = product.imageUrl
        ? `https://localhost:7175${product.imageUrl}`
        : "https://via.placeholder.com/400x400?text=LOI+Cosmetics";

    return (
        <div className="product-card">
            <div className="product-img">
                <img src={imageUrl} alt={product.name} />
            </div>

            <div className="product-info">
                <h3>{product.name}</h3>
                <div className="price">
                    {Number(product.price).toLocaleString("vi-VN")} đ
                </div>

                <Link className="btn-detail" to={`/product/${product.id}`}>
                    Xem chi tiết
                </Link>
            </div>
        </div>
    );
}

export default ProductCard;