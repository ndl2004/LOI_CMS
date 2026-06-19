import { useEffect, useState } from "react";
import { useSearchParams } from "react-router-dom";
import axiosClient from "../api/axiosClient";
import ProductCard from "../components/ProductCard";

function Shop() {
    const [categories, setCategories] = useState([]);
    const [products, setProducts] = useState([]);
    const [activeCategory, setActiveCategory] = useState(0);
    const [currentPage, setCurrentPage] = useState(1);

    const [minPrice, setMinPrice] = useState("");
    const [maxPrice, setMaxPrice] = useState("");
    const [searchKeyword, setSearchKeyword] = useState("");
    const [searchParams, setSearchParams] = useSearchParams();

    const pageSize = 8;

    const loadFilteredProducts = ({
        keyword = searchKeyword,
        min = minPrice,
        max = maxPrice,
        categoryId = activeCategory,
    } = {}) => {
        const params = {};

        if (keyword.trim()) {
            params.keyword = keyword.trim();
        }

        if (min !== "") {
            params.minPrice = Number(min);
        }

        if (max !== "") {
            params.maxPrice = Number(max);
        }

        if (categoryId > 0) {
            params.categoryProductId = categoryId;
        }

        axiosClient
            .get("/Products/filter", { params })
            .then((res) => setProducts(res.data))
            .catch((err) => {
                console.error("Lỗi lọc sản phẩm:", err);
                alert("Lọc sản phẩm thất bại. Vui lòng kiểm tra lại thông tin nhập.");
            });
    };

    const loadAllProducts = () => {
        setActiveCategory(0);
        setCurrentPage(1);
        setMinPrice("");
        setMaxPrice("");
        setSearchKeyword("");
        setSearchParams({});

        axiosClient.get("/Products").then((res) => setProducts(res.data));
    };

    const loadByCategory = (id) => {
        setActiveCategory(id);
        setCurrentPage(1);
        setMinPrice("");
        setMaxPrice("");
        setSearchKeyword("");
        setSearchParams(id > 0 ? { category: String(id) } : {});

        loadFilteredProducts({
            keyword: "",
            min: "",
            max: "",
            categoryId: id,
        });
    };

    const handleSearchKeywordChange = (e) => {
        const value = e.target.value;

        setSearchKeyword(value);
        setCurrentPage(1);

        loadFilteredProducts({
            keyword: value,
        });
    };

    const handleMinPriceChange = (e) => {
        const value = e.target.value;

        setMinPrice(value);
        setCurrentPage(1);

        loadFilteredProducts({
            min: value,
        });
    };

    const handleMaxPriceChange = (e) => {
        const value = e.target.value;

        setMaxPrice(value);
        setCurrentPage(1);

        loadFilteredProducts({
            max: value,
        });
    };

    const handleFilterPrice = () => {
        setCurrentPage(1);
        loadFilteredProducts();
    };

    const resetFilter = () => {
        loadAllProducts();
    };

    useEffect(() => {
        axiosClient.get("/CategoriesProducts").then((res) => setCategories(res.data));
    }, []);

    useEffect(() => {
        const categoryId = Number(searchParams.get("category")) || 0;

        setCurrentPage(1);

        if (categoryId > 0) {
            setActiveCategory(categoryId);
            setMinPrice("");
            setMaxPrice("");
            setSearchKeyword("");

            axiosClient
                .get("/Products/filter", {
                    params: { categoryProductId: categoryId },
                })
                .then((res) => setProducts(res.data))
                .catch((err) => {
                    console.error("Lỗi lọc sản phẩm theo danh mục:", err);
                    setProducts([]);
                });

            return;
        }

        setActiveCategory(0);
        axiosClient.get("/Products").then((res) => setProducts(res.data));
    }, [searchParams]);

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
                    <div className="shop-live-search">
                        <label>Tìm sản phẩm</label>
                        <input
                            type="text"
                            value={searchKeyword}
                            onChange={handleSearchKeywordChange}
                            placeholder="Nhập tên sản phẩm..."
                        />
                    </div>

                    <div>
                        <label>Giá từ</label>
                        <input
                            type="number"
                            value={minPrice}
                            onChange={handleMinPriceChange}
                            placeholder="0"
                        />
                    </div>

                    <div>
                        <label>Giá đến</label>
                        <input
                            type="number"
                            value={maxPrice}
                            onChange={handleMaxPriceChange}
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
                    <div className="empty-state">
                        <img
                            src="https://cdn-icons-png.flaticon.com/512/6134/6134065.png"
                            alt="Không tìm thấy sản phẩm"
                        />

                        <h3>Không tìm thấy sản phẩm nào phù hợp với tiêu chí của bạn</h3>

                        <p>
                            Hãy thử thay đổi khoảng giá hoặc chọn danh mục khác.
                        </p>
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
