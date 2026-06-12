import { Link, useNavigate } from "react-router-dom";

function ProductCard({
    product,
    tag = "",
    isFlashDeal = false,
    discountPercent = 0,
}) {
    const navigate = useNavigate();

    const imageUrl = product.imageUrl
        ? `https://localhost:7175${product.imageUrl}`
        : "https://via.placeholder.com/400x400?text=LOI+Cosmetics";

    const originalPrice = Number(product.price);

    const salePrice = isFlashDeal
        ? originalPrice - (originalPrice * discountPercent) / 100
        : originalPrice;

    const handleBuyNow = () => {
        let cart = JSON.parse(localStorage.getItem("cart")) || [];

        const existingItem = cart.find(
            (item) => item.id === product.id
        );

        if (existingItem) {
            existingItem.quantity += 1;
        } else {
            cart.push({
                id: product.id,
                name: product.name,
                price: salePrice,
                imageUrl: product.imageUrl,
                quantity: 1,
            });
        }

        localStorage.setItem("cart", JSON.stringify(cart));

        window.dispatchEvent(
            new Event("cartUpdated")
        );

        navigate("/checkout");
    };

    return (
        <div className="product-card">

            {tag && (
                <span
                    className={
                        tag === "HOT"
                            ? "hot-tag"
                            : "best-tag"
                    }
                >
                    {tag === "HOT"
                        ? "🔥 HOT"
                        : "⭐ BEST SELLER"}
                </span>
            )}

            {isFlashDeal && (
                <span className="sale-tag">
                    -{discountPercent}%
                </span>
            )}

            <div className="product-img">
                <img
                    src={imageUrl}
                    alt={product.name}
                />
            </div>

            <div className="product-info">
                <h3>{product.name}</h3>

                {isFlashDeal ? (
                    <>
                        <div className="old-price">
                            {originalPrice.toLocaleString("vi-VN")} đ
                        </div>

                        <div className="price">
                            {salePrice.toLocaleString("vi-VN")} đ
                        </div>
                    </>
                ) : (
                    <div className="price">
                        {originalPrice.toLocaleString("vi-VN")} đ
                    </div>
                )}

                <div className="sold-count">
                    Đã bán: {product.soldQuantity || 0}
                </div>

                <div className="product-actions">

                    <Link
                        className="btn-detail"
                        to={`/product/${product.id}`}
                    >
                        Xem chi tiết
                    </Link>

                    <button
                        className="btn-buy-now"
                        onClick={handleBuyNow}
                    >
                        Mua ngay
                    </button>

                </div>
            </div>
        </div>
    );
}

export default ProductCard;