import { useEffect, useState } from "react";
import axiosClient from "../api/axiosClient";
import ProductCard from "../components/ProductCard";

function Promotions() {
    const [activeFlashSale, setActiveFlashSale] = useState(null);
    const [countdown, setCountdown] = useState("00:00:00");

    useEffect(() => {
        axiosClient
            .get("/FlashSales/active")
            .then((res) => setActiveFlashSale(res.data))
            .catch(() => setActiveFlashSale(null));
    }, []);

    useEffect(() => {
        if (!activeFlashSale?.endTime) {
            setCountdown("00:00:00");
            return;
        }

        const updateCountdown = () => {
            const endTime = new Date(activeFlashSale.endTime).getTime();
            const remaining = Math.max(0, endTime - Date.now());
            const hours = Math.floor(remaining / (1000 * 60 * 60));
            const minutes = Math.floor((remaining / (1000 * 60)) % 60);
            const seconds = Math.floor((remaining / 1000) % 60);

            setCountdown(
                [hours, minutes, seconds]
                    .map((value) => value.toString().padStart(2, "0"))
                    .join(":")
            );
        };

        updateCountdown();
        const timerId = setInterval(updateCountdown, 1000);

        return () => clearInterval(timerId);
    }, [activeFlashSale]);

    const flashSaleItems = activeFlashSale?.items || [];

    return (
        <div className="container promotions-page">
            <section className="section-box flash-section">
                <div className="flash-header">
                    <h2>{activeFlashSale?.name || "Khuyến mãi"}</h2>

                    {flashSaleItems.length > 0 && (
                        <div className="flash-timer">
                            <span>Ưu đãi kết thúc sau</span>
                            <strong>{countdown}</strong>
                        </div>
                    )}
                </div>

                {flashSaleItems.length > 0 ? (
                    <div className="product-grid">
                        {flashSaleItems.map((item) => (
                            <ProductCard
                                key={item.id}
                                product={{
                                    id: item.productId,
                                    name: item.name,
                                    price: item.price,
                                    imageUrl: item.imageUrl,
                                    soldQuantity: item.productSoldQuantity,
                                }}
                                isFlashDeal={true}
                                discountPercent={item.discountPercent}
                                isSoldOut={item.isSoldOut}
                                saleQuantity={item.saleQuantity}
                                remainingQuantity={item.remainingQuantity}
                            />
                        ))}
                    </div>
                ) : (
                    <p className="promotion-empty">
                        Hiện chưa có chương trình khuyến mãi đang diễn ra.
                    </p>
                )}
            </section>
        </div>
    );
}

export default Promotions;
