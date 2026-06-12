import { useEffect, useState } from "react";
import axiosClient from "../api/axiosClient";
import ProductCard from "../components/ProductCard";
import LatestBlog from "../components/LatestBlog";
import HeroBanner from "../components/HeroBanner";

function Home() {
    const [categories, setCategories] = useState([]);
    const [products, setProducts] = useState([]);
    const [posts, setPosts] = useState([]);
    const [selectedCategoryId, setSelectedCategoryId] = useState(null);

    useEffect(() => {
        axiosClient.get("/CategoriesProducts").then((res) => setCategories(res.data));
        axiosClient.get("/Products").then((res) => setProducts(res.data));
        axiosClient.get("/Posts").then((res) => setPosts(res.data));
    }, []);

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

                    <div className="category-list">
                        <button
                            className={
                                selectedCategoryId === null
                                    ? "category-pill active"
                                    : "category-pill"
                            }
                            onClick={() => setSelectedCategoryId(null)}
                        >
                            Tất cả
                        </button>

                        {categories.map((cat) => (
                            <button
                                className={
                                    selectedCategoryId === cat.id
                                        ? "category-pill active"
                                        : "category-pill"
                                }
                                key={cat.id}
                                onClick={() => setSelectedCategoryId(cat.id)}
                            >
                                {cat.name}
                            </button>
                        ))}
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

                <section className="section-box flash-section">
                    <div className="flash-header">
                        <h2>Flash Deals</h2>

                        <div className="flash-timer">
                            <span>Ưu đãi kết thúc sau</span>
                            <strong>02:59:59</strong>
                        </div>
                    </div>

                    <div className="product-grid">
                        {products.slice(0, 4).map((item) => (
                            <ProductCard
                                key={item.id}
                                product={item}
                                isFlashDeal={true}
                                discountPercent={20}
                            />
                        ))}
                    </div>
                </section>

                <LatestBlog posts={posts} />
            </div>
        </>
    );
}

export default Home;