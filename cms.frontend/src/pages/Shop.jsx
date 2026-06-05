import { useEffect, useState } from "react";
import axiosClient from "../api/axiosClient";
import ProductCard from "../components/ProductCard";

function Shop() {
    const [categories, setCategories] = useState([]);
    const [products, setProducts] = useState([]);
    const [activeCategory, setActiveCategory] = useState(0);

    const loadAllProducts = () => {
        setActiveCategory(0);
        axiosClient.get("/Products").then((res) => setProducts(res.data));
    };

    const loadByCategory = (id) => {
        setActiveCategory(id);
        axiosClient
            .get(`/Products/category/${id}`)
            .then((res) => setProducts(res.data));
    };

    useEffect(() => {
        axiosClient.get("/CategoriesProducts").then((res) => setCategories(res.data));
        loadAllProducts();
    }, []);

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
            </section>

            <section className="section-box">
                <div className="section-head">
                    <h2>Danh sách sản phẩm</h2>
                    <span>{products.length} sản phẩm</span>
                </div>

                <div className="product-grid">
                    {products.map((item) => (
                        <ProductCard key={item.id} product={item} />
                    ))}
                </div>
            </section>
        </div>
    );
}

export default Shop;