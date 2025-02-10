db.createUser(
    {
        user: "${DATABASE_USERNAME}",
        pwd: "${DATABASE_PASSWORD}",
        roles: [
            {
                role: "readWrite",
                db: "${DATABASE_NAME}"
            }
        ]
    }
);
db.createCollection("products");
db.createCollection("categories");
db.createCollection("suppliers");

const categoryIds = db.categories.insertMany([
    { name: "Electronics", description: "Electronic gadgets and devices" },
    { name: "Clothing", description: "Apparel and accessories" }
]).insertedIds;

const supplierIds = db.suppliers.insertMany([
    {
        name: "Tech Distributors Inc.",
        contact: "John Doe",
        phone: "+123456789"
    },
    {
        name: "Gadget World Ltd.",
        contact: "Jane Smith",
        phone: "+987654321"
    }
]).insertedIds;

db.products.insertMany([
    {
        name: "Laptop",
        category_id: categoryIds[0],
        price: 1200.00,
        stock: 10,
        supplier_id: supplierIds[0],
        description: "A powerful laptop",
        created_at: new Date()
    },
    {
        name: "Phone",
        category_id: categoryIds[0],
        price: 800.00,
        stock: 25,
        supplier_id: supplierIds[1],
        description: "A high-end smartphone",
        created_at: new Date()
    },
    {
        name: "T-Shirt",
        category_id: categoryIds[1],
        price: 25.00,
        stock: 50,
        supplier_id: supplierIds[1],
        description: "A comfortable cotton T-shirt",
        created_at: new Date()
    }
]);