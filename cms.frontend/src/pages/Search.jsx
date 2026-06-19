import { useEffect, useState } from "react";
import { useSearchParams } from "react-router-dom";
import axiosClient from "../api/axiosClient";
import ProductCard from "../components/ProductCard";

function Search() {
    const [searchParams] = useSearchParams();
    const [products, setProducts] = useState([]);

    const keyword = searchParams.get("keyword");

    useEffect(() => {
        if (!keyword) {
            setProducts([]);
            return;
        }

        axiosClient
            .get("/Products/search", {
                params: {
                    keyword,
                },
            })
            .then((res) => setProducts(res.data))
            .catch(console.error);

    }, [keyword]);

    return (
        <div className="container">
            <section className="section-box">

                <div className="section-head">
                    <h2>
                        Kết quả tìm kiếm:
                        "{keyword}"
                    </h2>

                    <span>
                        {products.length} sản phẩm
                    </span>
                </div>

                {products.length === 0 ? (
                    <div className="empty-state">
                        <img
                            src="https://cdn-icons-png.flaticon.com/512/6134/6134065.png"
                            alt="Không tìm thấy sản phẩm"
                        />

                        <h3>Không tìm thấy sản phẩm nào phù hợp với tiêu chí của bạn</h3>

                        <p>
                            Hãy thử tìm kiếm bằng từ khóa khác.
                        </p>
                    </div>
                ) : (
                    <div className="product-grid">
                        {products.map((item) => (
                            <ProductCard
                                key={item.id}
                                product={item}
                            />
                        ))}
                    </div>
                )}
            </section>
        </div>
    );
}

export default Search;
