import { useEffect, useState } from "react";
import axiosClient from "../api/axiosClient";

function HeroBanner() {
    const [banners, setBanners] = useState([]);
    const [current, setCurrent] = useState(0);

    useEffect(() => {
        axiosClient
            .get("/Advertisements")
            .then((res) => {
                setBanners(res.data);
            })
            .catch((err) => {
                console.error("Lỗi lấy banner:", err);
            });
    }, []);

    useEffect(() => {
        if (banners.length === 0) return;

        const timer = setInterval(() => {
            setCurrent((prev) => (prev + 1) % banners.length);
        }, 4000);

        return () => clearInterval(timer);
    }, [banners]);

    if (banners.length === 0) {
        return null;
    }

    const item = banners[current];

    return (
        <section className="hero">
            <div className="container">
                <div className="hero-main">
                    <div className="hero-content">
                        <span className="hero-badge">
                            ✨ Mỹ phẩm chính hãng
                        </span>

                        <h1>{item.title}</h1>

                        <p>{item.description}</p>

                        <div className="hero-tags">
                            <span>100% Chính hãng</span>
                            <span>Đổi trả dễ dàng</span>
                            <span>Tư vấn chăm da</span>
                        </div>

                        <div className="hero-actions">
                            <button
                                onClick={() =>
                                    (window.location.href = item.link || "/shop")
                                }
                            >
                                Mua ngay
                            </button>

                            <a href={item.link || "/shop"}>
                                Xem sản phẩm
                            </a>
                        </div>
                    </div>

                    <div className="hero-image-box">
                        <img
                            src={`https://localhost:7175${item.image}`}
                            alt={item.title}
                            className="hero-image"
                        />
                    </div>
                </div>
            </div>
        </section>
    );
}

export default HeroBanner;