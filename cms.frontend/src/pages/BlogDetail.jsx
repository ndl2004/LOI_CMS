import { useEffect, useState } from "react";
import { useParams, Link } from "react-router-dom";
import axiosClient from "../api/axiosClient";

function BlogDetail() {
    const { id } = useParams();
    const [post, setPost] = useState(null);

    const getImageUrl = (imageUrl) => {
        if (!imageUrl) {
            return "https://via.placeholder.com/1000x500?text=LOI+Cosmetics";
        }

        if (imageUrl.startsWith("http")) {
            return imageUrl;
        }

        return `https://localhost:7175${imageUrl}`;
    };

    useEffect(() => {
        axiosClient
            .get(`/Posts/${id}`)
            .then((res) => setPost(res.data))
            .catch((err) => console.log("Lỗi Post Detail:", err));
    }, [id]);

    if (!post) {
        return (
            <div className="container">
                <section className="section-box" style={{ marginTop: 25 }}>
                    Đang tải bài viết...
                </section>
            </div>
        );
    }

    return (
        <div className="container">
            <section className="blog-detail-box">
                <span className="detail-category">
                    {post.categoryName || "Cẩm nang làm đẹp"}
                </span>

                <h1>{post.title}</h1>

                <img
                    className="blog-detail-img"
                    src={getImageUrl(post.imageUrl)}
                    alt={post.title}
                />

                <div
                    className="blog-content"
                    dangerouslySetInnerHTML={{
                        __html:
                            post.content ||
                            post.description ||
                            post.summary ||
                            "Bài viết chưa có nội dung chi tiết.",
                    }}
                />

                <Link className="btn-back-shop" to="/">
                    ← Quay lại trang chủ
                </Link>
            </section>
        </div>
    );
}

export default BlogDetail;