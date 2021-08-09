def de_casteljau(t, coefs):
    beta = [c for c in coefs] # values in this list are overridden
    n = len(beta)
    for j in range(1, n):
        for k in range(n - j):
            beta[k] = beta[k] * (1 - t) + beta[k + 1] * t
    return beta[0];

P1 = [-20,0,-11]
P2 = [-10,0,-5]
P3 = [10,0,-5]
P4 = [20,0,-11]
coefs_x = [P1[0],P2[0],P3[0],P4[0]]
coefs_y = [P1[1],P2[1],P3[1],P4[1]]
coefs_z = [P1[2],P2[2],P3[2],P4[2]]
beta = coefs_x
print('x =' + str(de_casteljau(0.7, coefs_x)))
print('y =' + str(de_casteljau(0.7, coefs_y)))
print('z =' + str(de_casteljau(0.7, coefs_z)))
