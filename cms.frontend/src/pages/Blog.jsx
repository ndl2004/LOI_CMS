import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import axiosClient from "../api/axiosClient";

function Blog() {
    const [posts, setPosts] = useState([]);

    const getImageUrl = (imageUrl) => {
        if (!imageUrl) return "https://via.placeholder.com/600x360?text=LOI+Cosmetics";
        if (imageUrl.startsWith("http")) return imageUrl;
        return `https://localhost:7175${imageUrl}`;
    };

    useEffect(() => {
        axiosClient
            .get("/Posts")
            .then((res) => setPosts(res.data))
            .catch((err) => console.log("Lỗi lấy bài viết:", err));
    }, []);

    return (
        <div className="container">
            <section className="blog-section">
                <div className="blog-head">
                    <div>
                        <span className="blog-subtitle">Beauty Blog</span>
                        <h2>Cẩm nang làm đẹp</h2>
                        <p>Tổng hợp các bài viết chăm sóc da và mỹ phẩm.</p>
                    </div>
                </div>

                <div className="blog-grid">
                    {posts.map((post) => (
                        <article className="blog-card" key={post.id}>
                            <div className="blog-image">
                                <img src={getImageUrl(post.imageUrl)} alt={post.title} />
                                <span className="blog-badge">
                                    {post.categoryName || "Làm đẹp"}
                                </span>
                            </div>

                            <div className="blog-content">
                                <div className="blog-date">
                                    {post.createdDate
                                        ? new Date(post.createdDate).toLocaleDateString("vi-VN")
                                        : "LOI Cosmetics"}
                                </div>

                                <h3>{post.title}</h3>

                                <p>
                                    {post.content
                                        ? post.content.substring(0, 100) + "..."
                                        : "Cập nhật kiến thức làm đẹp và chăm sóc da."}
                                </p>

                                <Link className="blog-read-more" to={`/blog/${post.id}`}>
                                    Đọc bài viết →
                                </Link>
                            </div>
                        </article>
                    ))}
                </div>
            </section>
        </div>
    );
}

export default Blog;