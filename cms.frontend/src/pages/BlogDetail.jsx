import { useEffect, useState } from "react";
import { useParams, Link } from "react-router-dom";
import axiosClient from "../api/axiosClient";
import { IMAGE_BASE_URL } from "../config";

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

        return `${IMAGE_BASE_URL}${imageUrl}`;
    };

    const formatContent = (html) => {
        if (!html) return "Bài viết chưa có nội dung chi tiết.";

        return html
            .replaceAll(
                'src="/uploads/',
                `src="${IMAGE_BASE_URL}/uploads/`
            )
            .replaceAll(
                "src='/uploads/",
                `src='${IMAGE_BASE_URL}/uploads/`
            );
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
                    className="blog-content blog-detail-content"
                    dangerouslySetInnerHTML={{
                        __html: formatContent(
                            post.content ||
                            post.description ||
                            post.summary
                        ),
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