import { useEffect, useState } from "react";
import axiosClient from "../api/axiosClient";
import ProductCard from "../components/ProductCard";
import LatestBlog from "../components/LatestBlog";
import HeroBanner from "../components/HeroBanner";
import { IMAGE_BASE_URL } from "../config";

function Home() {
    const [categories, setCategories] = useState([]);
    const [products, setProducts] = useState([]);
    const [posts, setPosts] = useState([]);
    const [selectedCategoryId, setSelectedCategoryId] = useState(null);
    const [activeFlashSale, setActiveFlashSale] = useState(null);
    const [flashCountdown, setFlashCountdown] = useState("00:00:00");

    useEffect(() => {
        axiosClient.get("/CategoriesProducts").then((res) => setCategories(res.data));
        axiosClient.get("/Products").then((res) => setProducts(res.data));
        axiosClient.get("/Posts").then((res) => setPosts(res.data));
        axiosClient.get("/FlashSales/active").then((res) => setActiveFlashSale(res.data));
    }, []);

    useEffect(() => {
        if (!activeFlashSale?.endTime) {
            setFlashCountdown("00:00:00");
            return;
        }

        const updateCountdown = () => {
            const endTime = new Date(activeFlashSale.endTime).getTime();
            const remaining = Math.max(0, endTime - Date.now());
            const hours = Math.floor(remaining / (1000 * 60 * 60));
            const minutes = Math.floor((remaining / (1000 * 60)) % 60);
            const seconds = Math.floor((remaining / 1000) % 60);

            setFlashCountdown(
                [hours, minutes, seconds]
                    .map((value) => value.toString().padStart(2, "0"))
                    .join(":")
            );
        };

        updateCountdown();
        const timerId = setInterval(updateCountdown, 1000);

        return () => clearInterval(timerId);
    }, [activeFlashSale]);

    const getSoldCount = (product) => {
        return (
            product.soldQuantity ||
            product.quantitySold ||
            product.buyCount ||
            product.sold ||
            product.totalSold ||
            0
        );
    };

    const featuredProducts = [...products]
        .sort((a, b) => getSoldCount(b) - getSoldCount(a))
        .slice(0, 4);

    const hotProductId =
        featuredProducts.length > 0 ? featuredProducts[0].id : null;

    const filteredProducts = selectedCategoryId
        ? products.filter((item) =>
            item.categoryProductId === selectedCategoryId ||
            item.categoryId === selectedCategoryId ||
            item.categoryProductID === selectedCategoryId
        )
        : featuredProducts;

    const selectedCategory = categories.find(
        (cat) => cat.id === selectedCategoryId
    );
    const newestProducts = [...products]
        .sort((a, b) => b.id - a.id)
        .slice(0, 3);
    const flashSaleItems = activeFlashSale?.items || [];

    return (
        <>
            <HeroBanner />

            <div className="container">
                <div className="service-row">
                    <div className="service-item">
                        <div className="service-icon">4.6</div>
                        Deal chất
                    </div>

                    <div className="service-item">
                        <div className="service-icon">2H</div>
                        Giao nhanh
                    </div>

                    <div className="service-item">
                        <div className="service-icon">100%</div>
                        Chính hãng
                    </div>

                    <div className="service-item">
                        <div className="service-icon">SPA</div>
                        Chăm da
                    </div>

                    <div className="service-item">
                        <div className="service-icon">SALE</div>
                        Khuyến mãi
                    </div>

                    <div className="service-item">
                        <div className="service-icon">BLOG</div>
                        Cẩm nang
                    </div>
                </div>

                <section className="section-box">
                    <div className="section-head">
                        <h2>Danh mục mỹ phẩm</h2>
                    </div>

                    <div className="category-card-list">
                        <button
                            className={
                                selectedCategoryId === null
                                    ? "category-card active"
                                    : "category-card"
                            }
                            onClick={() => setSelectedCategoryId(null)}
                        >
                            <div className="category-card-img">
                                ✨
                            </div>

                            <span>Tất cả</span>
                        </button>

                        {categories.map((cat) => {
                            const imageUrl = cat.imageUrl
                                ? `${IMAGE_BASE_URL}${cat.imageUrl}`
                                : "https://via.placeholder.com/120x120?text=LOI";

                            return (
                                <button
                                    key={cat.id}
                                    className={
                                        selectedCategoryId === cat.id
                                            ? "category-card active"
                                            : "category-card"
                                    }
                                    onClick={() => setSelectedCategoryId(cat.id)}
                                >
                                    <div className="category-card-img">
                                        <img src={imageUrl} alt={cat.name} />
                                    </div>

                                    <span>{cat.name}</span>
                                </button>
                            );
                        })}
                    </div>
                </section>

                <section className="section-box">
                    <div className="section-head">
                        <h2>
                            {selectedCategory
                                ? `Sản phẩm: ${selectedCategory.name}`
                                : "Sản phẩm nổi bật"}
                        </h2>

                        <a href="/shop">Xem tất cả</a>
                    </div>

                    <div className="product-grid">
                        {filteredProducts.length > 0 ? (
                            filteredProducts.map((item, index) => (
                                <ProductCard
                                    key={item.id}
                                    product={item}
                                    tag={
                                        selectedCategoryId === null
                                            ? index === 0
                                                ? "HOT"
                                                : "BEST SELLER"
                                            : ""
                                    }
                                />
                            ))
                        ) : (
                            <p>Không có sản phẩm trong danh mục này.</p>
                        )}
                    </div>
                </section>
                <section className="section-box">
                    <div className="section-head">
                        <h2>Sản phẩm mới nhất</h2>
                        <a href="/shop">Xem tất cả</a>
                    </div>

                    <div className="product-grid">
                        {newestProducts.map((item) => (
                            <ProductCard
                                key={item.id}
                                product={item}
                                tag="NEW"
                            />
                        ))}
                    </div>
                </section>
                {flashSaleItems.length > 0 && (
                    <section className="section-box flash-section">
                        <div className="flash-header">
                            <h2>{activeFlashSale?.name || "Flash Deals"}</h2>

                            <div className="flash-timer">
                                <span>Ưu đãi kết thúc sau</span>
                                <strong>{flashCountdown}</strong>
                            </div>

                            <a className="flash-more-link" href="/promotions">
                                Xem thêm khuyến mãi
                            </a>
                        </div>

                        <div className="product-grid">
                            {flashSaleItems.slice(0, 4).map((item) => (
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
                    </section>
                )}

                <LatestBlog posts={posts} />
            </div>
        </>
    );
}

export default Home;
