import { Link, useNavigate } from "react-router-dom";
import { IMAGE_BASE_URL } from "../config";

function ProductCard({
    product,
    tag = "",
    isFlashDeal = false,
    discountPercent = 0,
    isSoldOut = false,
    saleQuantity = 0,
    remainingQuantity = null,
}) {
    const navigate = useNavigate();

    const imageUrl = product.imageUrl
        ? `${IMAGE_BASE_URL}${product.imageUrl}`
        : "https://via.placeholder.com/400x400?text=LOI+Cosmetics";

    const originalPrice = Number(product.price);

    const salePrice = isFlashDeal
        ? originalPrice - (originalPrice * discountPercent) / 100
        : originalPrice;

    const handleBuyNow = () => {
        if (isSoldOut) {
            return;
        }

        let cart = JSON.parse(localStorage.getItem("cart")) || [];

        const existingItem = cart.find(
            (item) => item.id === product.id
        );

        if (existingItem) {
            existingItem.quantity += 1;
            existingItem.price = salePrice;
            existingItem.originalPrice = originalPrice;
            existingItem.isFlashDeal = isFlashDeal;
            existingItem.discountPercent = discountPercent;
        } else {
            cart.push({
                id: product.id,
                name: product.name,
                price: salePrice,
                originalPrice,
                imageUrl: product.imageUrl,
                quantity: 1,
                isFlashDeal,
                discountPercent,
            });
        }

        localStorage.setItem("cart", JSON.stringify(cart));
        window.dispatchEvent(new Event("cartUpdated"));

        navigate("/checkout");
    };

    return (
        <div className={isSoldOut ? "product-card sold-out-card" : "product-card"}>
            {tag && (
                <span
                    className={
                        tag === "HOT"
                            ? "hot-tag"
                            : tag === "NEW"
                                ? "new-tag"
                                : "best-tag"
                    }
                >
                    {tag === "HOT"
                        ? "🔥 HOT"
                        : tag === "NEW"
                            ? "🆕 NEW"
                            : "⭐ BEST SELLER"}
                </span>
            )}

            {isFlashDeal && (
                <span className="sale-tag">
                    -{discountPercent}%
                </span>
            )}

            {isSoldOut && (
                <span className="sold-out-tag">
                    Hết lượt khuyến mãi
                </span>
            )}

            <div className="product-img">
                <img src={imageUrl} alt={product.name} />
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

                {isFlashDeal && (
                    <div className={isSoldOut ? "flash-stock sold-out" : "flash-stock"}>
                        {isSoldOut
                            ? "Hết lượt khuyến mãi"
                            : saleQuantity === 0
                                ? "Không giới hạn suất"
                                : `Còn ${remainingQuantity} suất`}
                    </div>
                )}

                <div className="product-actions">
                    <Link className="btn-detail" to={`/product/${product.id}`}>
                        Xem chi tiết
                    </Link>

                    <button
                        className="btn-buy-now"
                        onClick={handleBuyNow}
                        disabled={isSoldOut}
                    >
                        {isSoldOut ? "Đã hết" : "Mua ngay"}
                    </button>
                </div>
            </div>
        </div>
    );
}

export default ProductCard;
