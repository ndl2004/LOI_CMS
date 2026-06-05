import { useEffect, useState } from "react";
import axiosClient from "../api/axiosClient";
import ProductCard from "../components/ProductCard";
import LatestBlog from "../components/LatestBlog";

function Home() {
    const [categories, setCategories] = useState([]);
    const [products, setProducts] = useState([]);
    const [posts, setPosts] = useState([]);

    useEffect(() => {
        axiosClient.get("/CategoriesProducts").then((res) => setCategories(res.data));
        axiosClient.get("/Products").then((res) => setProducts(res.data));
        axiosClient.get("/Posts").then((res) => setPosts(res.data));
    }, []);

    return (
        <>
            <section className="hero">
                <div className="container hero-grid">
                    <div className="hero-main">
                        <h1>Chăm sóc da chuẩn xinh mỗi ngày</h1>
                        <p>
                            Khám phá mỹ phẩm chính hãng, serum, kem dưỡng, sữa rửa mặt,
                            kem chống nắng và nhiều sản phẩm làm đẹp chất lượng.
                        </p>
                        <button onClick={() => (window.location.href = "/shop")}>
                            Mua ngay
                        </button>
                    </div>

                    <div className="hero-side">
                        <div className="side-card">
                            <h3>Giao nhanh 2H</h3>
                            <p>Áp dụng tại khu vực nội thành.</p>
                        </div>
                        <div className="side-card">
                            <h3>Freeship</h3>
                            <p>Ưu đãi vận chuyển cho đơn hàng mỹ phẩm.</p>
                        </div>
                    </div>
                </div>
            </section>

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
                        {categories.map((cat) => (
                            <span className="category-pill" key={cat.id}>
                                {cat.name}
                            </span>
                        ))}
                    </div>
                </section>

                <section className="section-box flash-section">
                    <div className="section-head">
                        <h2 style={{ color: "#fff" }}>Flash Deals</h2>
                        <strong>Ưu đãi hôm nay</strong>
                    </div>

                    <div className="product-grid">
                        {products.slice(0, 4).map((item) => (
                            <ProductCard key={item.id} product={item} />
                        ))}
                    </div>
                </section>

                <section className="section-box">
                    <div className="section-head">
                        <h2>Sản phẩm nổi bật</h2>
                        <a href="/shop">Xem tất cả</a>
                    </div>

                    <div className="product-grid">
                        {products.slice(0, 8).map((item) => (
                            <ProductCard key={item.id} product={item} />
                        ))}
                    </div>
                </section>

                <LatestBlog posts={posts} />
            </div>
        </>
    );
}

export default Home;