import { useEffect, useState } from "react";
import axiosClient from "../api/axiosClient";
import ProductCard from "../components/ProductCard";

function Shop() {
    const [categories, setCategories] = useState([]);
    const [products, setProducts] = useState([]);
    const [activeCategory, setActiveCategory] = useState(0);
    const [currentPage, setCurrentPage] = useState(1);

    const [minPrice, setMinPrice] = useState("");
    const [maxPrice, setMaxPrice] = useState("");

    const pageSize = 8;

    const loadAllProducts = () => {
        setActiveCategory(0);
        setCurrentPage(1);
        setMinPrice("");
        setMaxPrice("");

        axiosClient.get("/Products").then((res) => setProducts(res.data));
    };

    const loadByCategory = (id) => {
        setActiveCategory(id);
        setCurrentPage(1);
        setMinPrice("");
        setMaxPrice("");

        axiosClient
            .get(`/Products/category/${id}`)
            .then((res) => setProducts(res.data));
    };

    const handleFilterPrice = () => {
        setActiveCategory(0);
        setCurrentPage(1);

        const params = {};

        if (minPrice !== "") {
            params.minPrice = Number(minPrice);
        }

        if (maxPrice !== "") {
            params.maxPrice = Number(maxPrice);
        }

        axiosClient
            .get("/Products/filter-price", { params })
            .then((res) => setProducts(res.data))
            .catch((err) => {
                console.error("Lỗi lọc giá:", err);
                alert("Lọc giá thất bại. Vui lòng kiểm tra lại giá nhập.");
            });
    };

    const resetFilter = () => {
        loadAllProducts();
    };

    useEffect(() => {
        axiosClient.get("/CategoriesProducts").then((res) => setCategories(res.data));
        loadAllProducts();
    }, []);

    const totalPages = Math.ceil(products.length / pageSize);

    const pagedProducts = products.slice(
        (currentPage - 1) * pageSize,
        currentPage * pageSize
    );

    return (
        <div className="container">
            <section className="section-box" style={{ marginTop: 25 }}>
                <div className="section-head">
                    <h2>Cửa hàng mỹ phẩm</h2>
                </div>

                <div className="category-list">
                    <button
                        className={activeCategory === 0 ? "category-pill active" : "category-pill"}
                        onClick={loadAllProducts}
                    >
                        Tất cả
                    </button>

                    {categories.map((cat) => (
                        <button
                            key={cat.id}
                            className={
                                activeCategory === cat.id
                                    ? "category-pill active"
                                    : "category-pill"
                            }
                            onClick={() => loadByCategory(cat.id)}
                        >
                            {cat.name}
                        </button>
                    ))}
                </div>

                <div className="price-filter">
                    <div>
                        <label>Giá từ</label>
                        <input
                            type="number"
                            value={minPrice}
                            onChange={(e) => setMinPrice(e.target.value)}
                            placeholder="0"
                        />
                    </div>

                    <div>
                        <label>Giá đến</label>
                        <input
                            type="number"
                            value={maxPrice}
                            onChange={(e) => setMaxPrice(e.target.value)}
                            placeholder="500000"
                        />
                    </div>

                    <button onClick={handleFilterPrice}>
                        Lọc giá
                    </button>

                    <button className="btn-reset-filter" onClick={resetFilter}>
                        Xóa lọc
                    </button>
                </div>
            </section>

            <section className="section-box">
                <div className="section-head">
                    <h2>Danh sách sản phẩm</h2>
                    <span>{products.length} sản phẩm</span>
                </div>

                {products.length === 0 ? (
                    <div className="empty-search">
                        <div className="empty-icon">🔍</div>
                        <h3>Không tìm thấy sản phẩm nào phù hợp với tiêu chí của bạn</h3>
                    </div>
                ) : (
                    <>
                        <div className="product-grid">
                            {pagedProducts.map((item) => (
                                <ProductCard key={item.id} product={item} />
                            ))}
                        </div>

                        {totalPages > 1 && (
                            <div className="pagination">
                                <button
                                    className="pagination-nav"
                                    disabled={currentPage === 1}
                                    onClick={() => setCurrentPage(currentPage - 1)}
                                >
                                    ← Trước
                                </button>

                                {[...Array(totalPages)].map((_, index) => (
                                    <button
                                        key={index}
                                        className={currentPage === index + 1 ? "active" : ""}
                                        onClick={() => setCurrentPage(index + 1)}
                                    >
                                        {index + 1}
                                    </button>
                                ))}

                                <button
                                    className="pagination-nav"
                                    disabled={currentPage === totalPages}
                                    onClick={() => setCurrentPage(currentPage + 1)}
                                >
                                    Sau →
                                </button>
                            </div>
                        )}
                    </>
                )}
            </section>
        </div>
    );
}

export default Shop;